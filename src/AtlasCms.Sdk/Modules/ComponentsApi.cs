using AtlasCms.Sdk.Http;
using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Implementation of IComponentsApi providing component management operations.
/// </summary>
internal sealed class ComponentsApi(AtlasHttpClient http, string restBaseUrl) : AtlasRestModuleBase(http, restBaseUrl), IComponentsApi
{
    /// <inheritdoc />
    public Task<IReadOnlyList<Component>> ListAsync(
        AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<IReadOnlyList<Component>>(
            Get(Url("/content-types/components"), options), ct);

    /// <inheritdoc />
    public Task<Component> GetByIdAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<Component>(
            Get(Url($"/content-types/components/{Enc(id)}"), options), ct);

    /// <inheritdoc />
    public Task<KeyResult<string>> CreateAsync(
        CreateComponentInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        return http.RequestAsync<KeyResult<string>>(
            Post(Url("/content-types/components"), payload, options), ct);
    }

    /// <inheritdoc />
    public Task<KeyResult<string>> UpdateAsync(
        UpdateComponentInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        return http.RequestAsync<KeyResult<string>>(
            new HttpRequestConfig
            {
                Url = Url($"/content-types/components/{Enc(payload.Id)}"),
                Method = HttpMethod.Put,
                Body = payload,
                ApiKey = options?.ApiKey
            }, ct);
    }

    /// <inheritdoc />
    public Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/content-types/components/{Enc(id)}"), Method = HttpMethod.Delete, ApiKey = options?.ApiKey }, ct);

}
