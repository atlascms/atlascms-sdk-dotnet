using System.Net;
using System.Text.Json;

namespace AtlasCms.Sdk.Http;

public class AtlasHttpException : Exception
{
    public int StatusCode { get; }
    public string? Code { get; }
    public object? Details { get; }
    public string? RequestId { get; }
    public string? Raw { get; }

    public AtlasHttpException(string message, int statusCode, string? code = null, object? details = null, string? requestId = null, string? raw = null)
        : base(message)
    {
        StatusCode = statusCode;
        Code = code;
        Details = details;
        RequestId = requestId;
        Raw = raw;
    }

    internal static AtlasHttpException FromResponse(HttpStatusCode status, string? body, string? requestId = null)
    {
        var code = (int)status;
        if (string.IsNullOrWhiteSpace(body))
            return new AtlasHttpException($"Request failed with status {code}", code, requestId: requestId, raw: body);

        try
        {
            var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;
            var message = ReadString(root, "message") ?? ReadString(root, "title") ?? ReadString(root, "error")
                ?? $"Request failed with status {code}";
            var errorCode = ReadString(root, "code");
            object? details = root.TryGetProperty("details", out var d) ? (object)d :
                              root.TryGetProperty("errors", out var e) ? e : null;

            return new AtlasHttpException(message, code, errorCode, details, requestId, body);
        }
        catch
        {
            return new AtlasHttpException($"Request failed with status {code}", code, requestId: requestId, raw: body);
        }
    }

    private static string? ReadString(JsonElement el, string property)
        => el.TryGetProperty(property, out var p) && p.ValueKind == JsonValueKind.String ? p.GetString() : null;
}

public class AtlasNetworkException : Exception
{
    public AtlasNetworkException(string message, Exception? inner = null)
        : base(message, inner) { }
}

public class AtlasTimeoutException : Exception
{
    public TimeSpan Timeout { get; }

    public AtlasTimeoutException(TimeSpan timeout)
        : base($"Request timed out after {timeout.TotalMilliseconds}ms")
    {
        Timeout = timeout;
    }
}
