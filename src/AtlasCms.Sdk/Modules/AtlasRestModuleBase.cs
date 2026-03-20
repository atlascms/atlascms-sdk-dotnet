using AtlasCms.Sdk.Http;

namespace AtlasCms.Sdk.Modules;

internal abstract class AtlasRestModuleBase(AtlasHttpClient http, string restBaseUrl)
{
    protected AtlasHttpClient Http { get; } = http;

    protected string Url(string path, string? query = null)
    {
        var full = restBaseUrl.TrimEnd('/') + path;
        return string.IsNullOrEmpty(query) ? full : $"{full}?{query}";
    }

    protected static string Enc(string v) => Uri.EscapeDataString(v);

    protected static HttpRequestConfig Get(string url, AtlasRequestOptions? o = null) =>
        new() { Url = url, Method = HttpMethod.Get, ApiKey = o?.ApiKey };

    protected static HttpRequestConfig Post(string url, object? body, AtlasRequestOptions? o = null) =>
        new() { Url = url, Method = HttpMethod.Post, Body = body, ApiKey = o?.ApiKey };

    protected static HttpRequestConfig Put(string url, object? body, AtlasRequestOptions? o = null) =>
        new() { Url = url, Method = HttpMethod.Put, Body = body, ApiKey = o?.ApiKey };

    protected static HttpRequestConfig Delete(string url, AtlasRequestOptions? o = null) =>
        new() { Url = url, Method = HttpMethod.Delete, ApiKey = o?.ApiKey };
}

