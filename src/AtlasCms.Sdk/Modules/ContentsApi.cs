using AtlasCms.Sdk.Http;
using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Implementation of IContentsApi providing content management operations.
/// </summary>
internal sealed class ContentsApi(AtlasHttpClient http, string restBaseUrl) : IContentsApi
{
    /// <inheritdoc />
    public Task<PagedResult<Content>> ListAsync(
        string type, string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<PagedResult<Content>>(
            Get(Url($"/contents/{Enc(type)}", query), options), ct);

    /// <inheritdoc />
    public Task<Content> GetByIdAsync(
        string type, string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<Content>(
            Get(Url($"/contents/{Enc(type)}/{Enc(id)}"), options), ct);

    /// <inheritdoc />
    public Task<Content> GetSingleAsync(
        string type, string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<Content>(
            Get(Url($"/contents/{Enc(type)}/single", query), options), ct);

    /// <inheritdoc />
    public async Task<int> CountAsync(
        string type, string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var result = await http.RequestAsync<ApiKeyResult>(
            Get(Url($"/contents/{Enc(type)}/count", query), options), ct);
        return result?.IntValue ?? 0;
    }

    /// <inheritdoc />
    public async Task<CreateResult> CreateAsync(
        string type, CreateContentInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var result = await http.RequestAsync<ApiKeyResult>(
            Post(Url($"/contents/{Enc(type)}"), payload, options), ct);
        return new CreateResult { Id = result?.StringValue ?? string.Empty };
    }

    /// <inheritdoc />
    public Task UpdateAsync(
        string type, string id, UpdateContentInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/contents/{Enc(type)}/{Enc(id)}"), Method = HttpMethod.Put, Body = payload, ApiKey = options?.ApiKey }, ct);

    /// <inheritdoc />
    public Task RemoveAsync(
        string type, string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/contents/{Enc(type)}/{Enc(id)}"), Method = HttpMethod.Delete, ApiKey = options?.ApiKey }, ct);

    /// <inheritdoc />
    public Task ChangeStatusAsync(
        string type, string id, ContentStatus status, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            Post(Url($"/contents/{Enc(type)}/{Enc(id)}/status"),
                new { status = status.ToString().ToLowerInvariant() }, options), ct);

    /// <inheritdoc />
    public async Task<CreateResult> CreateTranslationAsync(
        string type, string id, string? locale = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var result = await http.RequestAsync<ApiKeyResult>(
            Post(Url($"/contents/{Enc(type)}/{Enc(id)}/create-translation"), new { locale }, options), ct);
        return new CreateResult { Id = result?.StringValue ?? string.Empty };
    }

    /// <inheritdoc />
    public async Task<CreateResult> DuplicateAsync(
        string type, string id, bool locales = false, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var result = await http.RequestAsync<ApiKeyResult>(
            Post(Url($"/contents/{Enc(type)}/{Enc(id)}/duplicate"), new { locales }, options), ct);
        return new CreateResult { Id = result?.StringValue ?? string.Empty };
    }

    /// <inheritdoc />
    public Task UpdateSeoAsync(
        string type, string id, UpdateContentSeoInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/contents/{Enc(type)}/{Enc(id)}/seo"), Method = HttpMethod.Post, Body = payload, ApiKey = options?.ApiKey }, ct);

    private string Url(string path, string? query = null)
    {
        var full = restBaseUrl.TrimEnd('/') + path;
        return string.IsNullOrEmpty(query) ? full : $"{full}?{query}";
    }

    private static string Enc(string v) => Uri.EscapeDataString(v);

    private static HttpRequestConfig Get(string url, AtlasRequestOptions? o = null) =>
        new() { Url = url, Method = HttpMethod.Get, ApiKey = o?.ApiKey };

    private static HttpRequestConfig Post(string url, object? body, AtlasRequestOptions? o = null) =>
        new() { Url = url, Method = HttpMethod.Post, Body = body, ApiKey = o?.ApiKey };
}

file record ApiKeyResult
{
    public string? Value { get; init; }
    public string? Key { get; init; }
    public string StringValue => Value ?? Key ?? string.Empty;
    public int IntValue => int.TryParse(Value, out var v) ? v : (int.TryParse(Key, out var k) ? k : 0);
}
