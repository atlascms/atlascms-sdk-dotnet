using AtlasCms.Sdk.Http;
using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Provides implementation for user management operations in the Atlas CMS system.
/// This class handles all HTTP communication for user-related API endpoints.
/// </summary>
internal sealed class UsersApi(AtlasHttpClient http, string restBaseUrl) : AtlasRestModuleBase(http, restBaseUrl), IUsersApi
{
    /// <inheritdoc />
    public Task<PagedResult<User>> ListAsync(
        string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => Http.RequestAsync<PagedResult<User>>(
            Get(Url("/users", query), options), ct);

    /// <inheritdoc />
    public async Task<int> CountAsync(
        string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var result = await Http.RequestAsync<KeyResult<int>>(
            Get(Url("/users/count", query), options), ct);
        return result?.Result ?? 0;
    }

    /// <inheritdoc />
    public Task<User> GetByIdAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => Http.RequestAsync<User>(
            Get(Url($"/users/{Enc(id)}"), options), ct);

    /// <inheritdoc />
    public Task<KeyResult<string>> CreateAsync(
        CreateUserInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        return Http.RequestAsync<KeyResult<string>>(
            Post(Url("/users/register"), payload, options), ct);
    }

    /// <inheritdoc />
    public Task UpdateAsync(
        string id, UpdateUserInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => Http.RequestVoidAsync(
            new HttpRequestConfig
            {
                Url = Url($"/users/{Enc(id)}"),
                Method = HttpMethod.Put,
                Body = new
                {
                    id,
                    firstName = payload.FirstName,
                    lastName = payload.LastName,
                    username = payload.Username,
                    email = payload.Email,
                    mobilePhone = payload.MobilePhone,
                    roles = payload.Roles,
                    notes = payload.Notes,
                    attributes = payload.Attributes
                },
                ApiKey = options?.ApiKey
            }, ct);

    /// <inheritdoc />
    public Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => Http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/users/{Enc(id)}"), Method = HttpMethod.Delete, ApiKey = options?.ApiKey }, ct);

    /// <inheritdoc />
    public Task ChangeStatusAsync(
        string id, bool isActive, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => Http.RequestVoidAsync(
            Post(Url($"/users/{Enc(id)}/status"), new { id, isActive }, options), ct);

    /// <inheritdoc />
    public Task ChangePasswordAsync(
        string id, string password, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => Http.RequestVoidAsync(
            Post(Url($"/users/{Enc(id)}/change-password"), new { id, password }, options), ct);
}
