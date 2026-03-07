using AtlasCms.Sdk.Http;
using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Provides implementation for media library management operations in the Atlas CMS system.
/// This class handles all HTTP communication for media-related API endpoints.
/// </summary>
internal sealed class MediaApi(AtlasHttpClient http, string restBaseUrl, string project) : IMediaApi
{
    private readonly string _project = Uri.EscapeDataString(project);

    /// <inheritdoc />
    public Task<PagedResult<Media>> ListAsync(
        string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<PagedResult<Media>>(
            Get(Url($"/{_project}/media-library/media", query), options), ct);

    /// <inheritdoc />
    public Task<Media> GetByIdAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<Media>(
            Get(Url($"/{_project}/media-library/media/{Enc(id)}"), options), ct);

    /// <inheritdoc />
    public Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/{_project}/media-library/media/{Enc(id)}"), Method = HttpMethod.Delete, ApiKey = options?.ApiKey }, ct);

    /// <inheritdoc />
    public Task SetTagsAsync(
        string id, IEnumerable<string> tags, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig
            {
                Url = Url($"/{_project}/media-library/media/{Enc(id)}/tags"),
                Method = HttpMethod.Post,
                Body = new { tags },
                ApiKey = options?.ApiKey
            }, ct);

    /// <inheritdoc />
    public Task<Media> UploadAsync(
        MediaUploadInput input, AtlasRequestOptions? options = null, CancellationToken ct = default)
    {
        var fileName = input.FileName ?? "file";
        var contentType = input.ContentType ?? "application/octet-stream";

        var form = new MultipartFormDataContent();
        var fileContent = new StreamContent(input.File);
        fileContent.Headers.ContentType = new(contentType);
        form.Add(fileContent, "file", fileName);

        if (input.Folder is not null) form.Add(new StringContent(input.Folder), "folder");
        if (input.FileName is not null) form.Add(new StringContent(input.FileName), "fileName");
        if (input.Id is not null) form.Add(new StringContent(input.Id), "id");

        return http.RequestAsync<Media>(
            new HttpRequestConfig
            {
                Url = Url($"/{_project}/media-library/media/upload"),
                Method = HttpMethod.Post,
                RawContent = form,
                ApiKey = options?.ApiKey
            }, ct);
    }

    /// <summary>
    /// Builds a complete URL from the provided path and optional query string.
    /// </summary>
    /// <param name="path">The API path to append to the base URL.</param>
    /// <param name="query">Optional query string to append to the URL.</param>
    /// <returns>The complete URL with base URL, path, and query string.</returns>
    private string Url(string path, string? query = null)
    {
        var full = restBaseUrl.TrimEnd('/') + path;
        return string.IsNullOrEmpty(query) ? full : $"{full}?{query}";
    }

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
}
