using AtlasCms.Sdk.Http;
using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Provides implementation for role management operations in the Atlas CMS system.
/// This class handles all HTTP communication for role-related API endpoints.
/// </summary>
internal sealed class RolesApi(AtlasHttpClient http, string restBaseUrl) : IRolesApi
{
    /// <inheritdoc />
    public Task<IReadOnlyList<Role>> ListAsync(
        AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<IReadOnlyList<Role>>(
            Get(Url("/users/roles"), options), ct);

    /// <inheritdoc />
    public async Task<CreateResult> CreateAsync(
        CreateRoleInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var result = await http.RequestAsync<ApiKeyResult>(
            Post(Url("/users/roles"), payload, options), ct);
        return new CreateResult { Id = result?.StringValue ?? string.Empty };
    }

    /// <inheritdoc />
    public Task UpdateAsync(
        string id, UpdateRoleInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/users/roles/{Enc(id)}"), Method = HttpMethod.Put, Body = payload, ApiKey = options?.ApiKey }, ct);

    /// <inheritdoc />
    public Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/users/roles/{Enc(id)}"), Method = HttpMethod.Delete, ApiKey = options?.ApiKey }, ct);

    /// <inheritdoc />
    public Task<IReadOnlyList<PermissionGroup>> GetPermissionsAsync(
        AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<IReadOnlyList<PermissionGroup>>(
            Get(Url("/users/roles/permissions"), options), ct);

    private string Url(string path)
        => restBaseUrl.TrimEnd('/') + path;

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
