using AtlasCms.Sdk.Http;
using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Provides implementation for user management operations in the Atlas CMS system.
/// This class handles all HTTP communication for user-related API endpoints.
/// </summary>
internal sealed class UsersApi(AtlasHttpClient http, string restBaseUrl) : IUsersApi
{
    /// <inheritdoc />
    public Task<PagedResult<User>> ListAsync(
        string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<PagedResult<User>>(
            Get(Url("/users", query), options), ct);

    /// <inheritdoc />
    public async Task<int> CountAsync(
        string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var result = await http.RequestAsync<ApiKeyResult>(
            Get(Url("/users/count", query), options), ct);
        return result?.IntValue ?? 0;
    }

    /// <inheritdoc />
    public Task<User> GetByIdAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<User>(
            Get(Url($"/users/{Enc(id)}"), options), ct);

    /// <inheritdoc />
    public async Task<CreateResult> CreateAsync(
        CreateUserInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var result = await http.RequestAsync<ApiKeyResult>(
            Post(Url("/users/register"), payload, options), ct);
        return new CreateResult { Id = result?.StringValue ?? string.Empty };
    }

    /// <inheritdoc />
    public Task UpdateAsync(
        string id, UpdateUserInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/users/{Enc(id)}"), Method = HttpMethod.Put, Body = payload, ApiKey = options?.ApiKey }, ct);

    /// <inheritdoc />
    public Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/users/{Enc(id)}"), Method = HttpMethod.Delete, ApiKey = options?.ApiKey }, ct);

    /// <inheritdoc />
    public Task ChangeStatusAsync(
        string id, bool isActive, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            Post(Url($"/users/{Enc(id)}/status"), new { isActive }, options), ct);

    /// <inheritdoc />
    public Task ChangePasswordAsync(
        string id, string password, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            Post(Url($"/users/{Enc(id)}/change-password"), new { password }, options), ct);

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
