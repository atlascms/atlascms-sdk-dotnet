using AtlasCms.Sdk.Http;
using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Implementation of IModelsApi providing content model management operations.
/// </summary>
internal sealed class ModelsApi(AtlasHttpClient http, string restBaseUrl) : AtlasRestModuleBase(http, restBaseUrl), IModelsApi
{
    /// <inheritdoc />
    public Task<IReadOnlyList<Model>> ListAsync(
        bool? system = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var q = system.HasValue ? $"system={(system.Value ? "true" : "false")}" : null;
        return Http.RequestAsync<IReadOnlyList<Model>>(
            Get(Url("/content-types/models", q), options), ct);
    }

    /// <inheritdoc />
    public Task<Model> GetByIdAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => Http.RequestAsync<Model>(
            Get(Url($"/content-types/models/{Enc(id)}"), options), ct);

    /// <inheritdoc />
    public Task<KeyResult<string>> CreateAsync(
        CreateModelInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        return Http.RequestAsync<KeyResult<string>>(
            Post(Url("/content-types/models"), payload, options), ct);
    }

    /// <inheritdoc />
    public Task<KeyResult<string>> UpdateAsync(
        UpdateModelInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        return Http.RequestAsync<KeyResult<string>>(
            new HttpRequestConfig
            {
                Url = Url($"/content-types/models/{Enc(payload.Id)}"),
                Method = HttpMethod.Put,
                Body = payload,
                ApiKey = options?.ApiKey
            }, ct);
    }

    /// <inheritdoc />
    public Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => Http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/content-types/models/{Enc(id)}"), Method = HttpMethod.Delete, ApiKey = options?.ApiKey }, ct);

}
