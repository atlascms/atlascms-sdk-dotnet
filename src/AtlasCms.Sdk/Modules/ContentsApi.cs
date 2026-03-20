using AtlasCms.Sdk.Http;
using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Implementation of IContentsApi providing content management operations.
/// </summary>
internal sealed class ContentsApi(AtlasHttpClient http, string restBaseUrl) : AtlasRestModuleBase(http, restBaseUrl), IContentsApi
{
    /// <inheritdoc />
    public Task<PagedResult<Content>> ListAsync(
        string type, string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<PagedResult<Content>>(
            Get(Url($"/contents/{Enc(type)}", query), options), ct);

    /// <inheritdoc />
    public Task<Content> GetByIdAsync(
        string type, string id, string? resolve = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var q = string.IsNullOrWhiteSpace(resolve) ? null : $"resolve={Uri.EscapeDataString(resolve)}";
        return http.RequestAsync<Content>(
            Get(Url($"/contents/{Enc(type)}/{Enc(id)}", q), options), ct);
    }

    /// <inheritdoc />
    public Task<Content> GetSingleAsync(
        string type, string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<Content>(
            Get(Url($"/contents/{Enc(type)}/single", query), options), ct);

    /// <inheritdoc />
    public async Task<int> CountAsync(
        string type, string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var result = await http.RequestAsync<KeyResult<int>>(
            Get(Url($"/contents/{Enc(type)}/count", query), options), ct);
        return result?.Result ?? 0;
    }

    /// <inheritdoc />
    public Task<KeyResult<string>> CreateAsync(
        string type, CreateContentInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        // Swagger requires `type` inside the request body for CreateContentCommand.
        var body = new
        {
            type,
            locale = payload.Locale,
            attributes = payload.Attributes
        };

        return http.RequestAsync<KeyResult<string>>(
            Post(Url($"/contents/{Enc(type)}"), body, options), ct);
    }

    /// <inheritdoc />
    public Task UpdateAsync(
        string type, string id, UpdateContentInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig
            {
                Url = Url($"/contents/{Enc(type)}/{Enc(id)}"),
                Method = HttpMethod.Put,
                // Swagger requires `id` and `type` inside the request body for UpdateContentCommand.
                Body = new { id, type, attributes = payload.Attributes, seo = payload.Seo },
                ApiKey = options?.ApiKey
            }, ct);

    /// <inheritdoc />
    public Task RemoveAsync(
        string type, string id, bool? locales = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var q = locales.HasValue ? $"locales={(locales.Value ? "true" : "false")}" : null;
        return http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/contents/{Enc(type)}/{Enc(id)}", q), Method = HttpMethod.Delete, ApiKey = options?.ApiKey }, ct);
    }

    /// <inheritdoc />
    public Task ChangeStatusAsync(
        string type, string id, ContentStatus status, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            Post(Url($"/contents/{Enc(type)}/{Enc(id)}/status"),
                new { id, type, status = status.ToString().ToLowerInvariant() }, options), ct);

    /// <inheritdoc />
    public Task<KeyResult<string>> CreateTranslationAsync(
        string type, string id, string? locale = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        return http.RequestAsync<KeyResult<string>>(
            Post(Url($"/contents/{Enc(type)}/{Enc(id)}/create-translation"), new { id, type, locale }, options), ct);
    }

    /// <inheritdoc />
    public Task<KeyResult<string>> DuplicateAsync(
        string type, string id, bool locales = false, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        return http.RequestAsync<KeyResult<string>>(
            Post(Url($"/contents/{Enc(type)}/{Enc(id)}/duplicate"), new { id, type, locales }, options), ct);
    }

    /// <inheritdoc />
    public Task UpdateSeoAsync(
        string type, string id, UpdateContentSeoInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig
            {
                Url = Url($"/contents/{Enc(type)}/{Enc(id)}/seo"),
                Method = HttpMethod.Post,
                // Swagger requires `id` and `type` inside the request body for UpdateContentSeoCommand.
                Body = new { id, type, seo = payload.Seo },
                ApiKey = options?.ApiKey
            }, ct);

}
