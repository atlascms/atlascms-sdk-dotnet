using AtlasCms.Sdk.Http;
using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Provides implementation for role management operations in the Atlas CMS system.
/// This class handles all HTTP communication for role-related API endpoints.
/// </summary>
internal sealed class RolesApi(AtlasHttpClient http, string restBaseUrl) : AtlasRestModuleBase(http, restBaseUrl), IRolesApi
{
    /// <inheritdoc />
    public Task<IReadOnlyList<Role>> ListAsync(
        AtlasRequestOptions? options = null, CancellationToken ct = default)
        => Http.RequestAsync<IReadOnlyList<Role>>(
            Get(Url("/users/roles"), options), ct);

    /// <inheritdoc />
    public Task<KeyResult<string>> CreateAsync(
        CreateRoleInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        return Http.RequestAsync<KeyResult<string>>(
            Post(Url("/users/roles"), payload, options), ct);
    }

    /// <inheritdoc />
    public Task UpdateAsync(
        string id, UpdateRoleInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => Http.RequestVoidAsync(
            new HttpRequestConfig
            {
                Url = Url($"/users/roles/{Enc(id)}"),
                Method = HttpMethod.Put,
                // Swagger requires `id` inside UpdateRoleCommand.
                Body = new { id, name = payload.Name, permissions = payload.Permissions },
                ApiKey = options?.ApiKey
            }, ct);

    /// <inheritdoc />
    public Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => Http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/users/roles/{Enc(id)}"), Method = HttpMethod.Delete, ApiKey = options?.ApiKey }, ct);

    /// <inheritdoc />
    public Task<IReadOnlyList<PermissionGroup>> GetPermissionsAsync(
        AtlasRequestOptions? options = null, CancellationToken ct = default)
        => Http.RequestAsync<IReadOnlyList<PermissionGroup>>(
            Get(Url("/users/roles/permissions"), options), ct);

}
