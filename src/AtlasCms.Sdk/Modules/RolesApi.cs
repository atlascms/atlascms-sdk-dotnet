using AtlasCms.Sdk.Http;
using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Provides implementation for role management operations in the Atlas CMS system.
/// This class handles all HTTP communication for role-related API endpoints.
/// </summary>
internal sealed class RolesApi(AtlasHttpClient http, string restBaseUrl, string project) : IRolesApi
{
    private readonly string _project = Uri.EscapeDataString(project);

    /// <inheritdoc />
    public Task<IReadOnlyList<Role>> ListAsync(
        AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<IReadOnlyList<Role>>(
            Get(Url($"/{_project}/users/roles"), options), ct);

    /// <inheritdoc />
    public async Task<CreateResult> CreateAsync(
        CreateRoleInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var result = await http.RequestAsync<ApiKeyResult>(
            Post(Url($"/{_project}/users/roles"), payload, options), ct);
        return new CreateResult { Id = result?.StringValue ?? string.Empty };
    }

    /// <inheritdoc />
    public Task UpdateAsync(
        string id, UpdateRoleInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/{_project}/users/roles/{Enc(id)}"), Method = HttpMethod.Put, Body = payload, ApiKey = options?.ApiKey }, ct);

    /// <inheritdoc />
    public Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/{_project}/users/roles/{Enc(id)}"), Method = HttpMethod.Delete, ApiKey = options?.ApiKey }, ct);

    /// <inheritdoc />
    public Task<IReadOnlyList<PermissionGroup>> GetPermissionsAsync(
        AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<IReadOnlyList<PermissionGroup>>(
            Get(Url($"/{_project}/users/roles/permissions"), options), ct);

    /// <summary>
    /// Builds a complete URL from the provided path.
    /// </summary>
    /// <param name="path">The API path to append to the base URL.</param>
    /// <returns>The complete URL with base URL and path.</returns>
    private string Url(string path)
        => restBaseUrl.TrimEnd('/') + path;

    /// <summary>
    /// URL-encodes a string value for safe inclusion in API URLs.
    /// </summary>
    /// <param name="v">The value to encode.</param>
    /// <returns>The URL-encoded value.</returns>
    private static string Enc(string v) => Uri.EscapeDataString(v);

    /// <summary>
    /// Creates an HTTP GET request configuration.
    /// </summary>
    /// <param name="url">The URL for the request.</param>
    /// <param name="o">Optional request options including API key override.</param>
    /// <returns>A configured HTTP request for a GET operation.</returns>
    private static HttpRequestConfig Get(string url, AtlasRequestOptions? o = null) =>
        new() { Url = url, Method = HttpMethod.Get, ApiKey = o?.ApiKey };

    /// <summary>
    /// Creates an HTTP POST request configuration.
    /// </summary>
    /// <param name="url">The URL for the request.</param>
    /// <param name="body">The request body to send with the POST operation.</param>
    /// <param name="o">Optional request options including API key override.</param>
    /// <returns>A configured HTTP request for a POST operation.</returns>
    private static HttpRequestConfig Post(string url, object? body, AtlasRequestOptions? o = null) =>
        new() { Url = url, Method = HttpMethod.Post, Body = body, ApiKey = o?.ApiKey };
}

file record ApiKeyResult
{
    public string? Value { get; init; }
    public string? Key { get; init; }
    public string StringValue => Value ?? Key ?? string.Empty;
}
