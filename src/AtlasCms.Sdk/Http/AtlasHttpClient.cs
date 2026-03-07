using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AtlasCms.Sdk.Http;

internal record HttpRequestConfig
{
    public required string Url { get; init; }
    public required HttpMethod Method { get; init; }
    public object? Body { get; init; }
    public HttpContent? RawContent { get; init; }
    public string? ApiKey { get; init; }
    public Dictionary<string, string>? Headers { get; init; }
    public TimeSpan? Timeout { get; init; }
}

internal class AtlasHttpClient
{
    private readonly HttpClient _http;
    private readonly string _defaultApiKey;

    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public AtlasHttpClient(AtlasClientConfig config)
    {
        _defaultApiKey = config.ApiKey;
        _http = config.HttpClient ?? new HttpClient();
    }

    public async Task<T> RequestAsync<T>(HttpRequestConfig config, CancellationToken ct = default)
    {
        using var cts = config.Timeout.HasValue
            ? CancellationTokenSource.CreateLinkedTokenSource(ct)
            : null;

        if (cts is not null && config.Timeout.HasValue)
            cts.CancelAfter(config.Timeout.Value);

        var token = cts?.Token ?? ct;

        try
        {
            using var request = BuildRequest(config);
            using var response = await _http.SendAsync(request, token);
            return await ParseResponseAsync<T>(response, token);
        }
        catch (AtlasHttpException) { throw; }
        catch (OperationCanceledException) when (config.Timeout.HasValue)
        {
            throw new AtlasTimeoutException(config.Timeout.Value);
        }
        catch (OperationCanceledException) { throw; }
        catch (Exception ex)
        {
            throw new AtlasNetworkException("Unable to reach Atlas CMS API", ex);
        }
    }

    public async Task RequestVoidAsync(HttpRequestConfig config, CancellationToken ct = default)
    {
        await RequestAsync<object?>(config, ct);
    }

    private HttpRequestMessage BuildRequest(HttpRequestConfig config)
    {
        var request = new HttpRequestMessage(config.Method, config.Url);

        var apiKey = config.ApiKey ?? _defaultApiKey;
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        if (config.Headers is not null)
            foreach (var (key, value) in config.Headers)
                request.Headers.TryAddWithoutValidation(key, value);

        if (config.RawContent is not null)
            request.Content = config.RawContent;
        else if (config.Body is not null)
            request.Content = JsonContent.Create(config.Body, options: JsonOptions);

        return request;
    }

    private static async Task<T> ParseResponseAsync<T>(HttpResponseMessage response, CancellationToken ct)
    {
        var requestId = response.Headers.TryGetValues("x-request-id", out var ids)
            ? string.Join(",", ids) : null;

        if (!response.IsSuccessStatusCode)
        {
            string? body = null;
            try { body = await response.Content.ReadAsStringAsync(ct); } catch { }
            throw AtlasHttpException.FromResponse(response.StatusCode, body, requestId);
        }

        if (response.StatusCode == HttpStatusCode.NoContent || typeof(T) == typeof(object))
            return default!;

        var content = await response.Content.ReadAsStringAsync(ct);
        if (string.IsNullOrWhiteSpace(content))
            return default!;

        return JsonSerializer.Deserialize<T>(content, JsonOptions)!;
    }
}
