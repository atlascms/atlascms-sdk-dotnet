using AtlasCms.Sdk.Http;
using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Implementation of IModelsApi providing content model management operations.
/// </summary>
internal sealed class ModelsApi(AtlasHttpClient http, string restBaseUrl) : IModelsApi
{
    /// <inheritdoc />
    public Task<PagedResult<Component>> ListAsync(
        string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<PagedResult<Component>>(
            Get(Url("/models", query), options), ct);

    /// <inheritdoc />
    public Task<Component> GetByIdAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<Component>(
            Get(Url($"/models/{Enc(id)}"), options), ct);

    /// <inheritdoc />
    public async Task<CreateResult> CreateAsync(
        CreateModelInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var result = await http.RequestAsync<ApiKeyResult>(
            Post(Url("/models"), payload, options), ct);
        return new CreateResult { Id = result?.StringValue ?? string.Empty };
    }

    /// <inheritdoc />
    public Task UpdateAsync(
        UpdateModelInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/models/{Enc(payload.Id)}"), Method = HttpMethod.Put, Body = payload, ApiKey = options?.ApiKey }, ct);

    /// <inheritdoc />
    public Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/models/{Enc(id)}"), Method = HttpMethod.Delete, ApiKey = options?.ApiKey }, ct);

    /// <inheritdoc />
    public Task PublishAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            Post(Url($"/models/{Enc(id)}/publish"), null, options), ct);

    /// <inheritdoc />
    public Task UnpublishAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            Post(Url($"/models/{Enc(id)}/unpublish"), null, options), ct);

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
}
