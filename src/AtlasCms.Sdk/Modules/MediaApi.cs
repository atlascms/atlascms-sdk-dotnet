using AtlasCms.Sdk.Http;
using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Provides implementation for media library management operations in the Atlas CMS system.
/// This class handles all HTTP communication for media-related API endpoints.
/// </summary>
internal sealed class MediaApi(AtlasHttpClient http, string restBaseUrl) : AtlasRestModuleBase(http, restBaseUrl), IMediaApi
{
    /// <inheritdoc />
    public Task<PagedResult<Media>> ListAsync(
        string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<PagedResult<Media>>(
            Get(Url("/media-library/media", query), options), ct);

    /// <inheritdoc />
    public Task<Media> GetByIdAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<Media>(
            Get(Url($"/media-library/media/{Enc(id)}"), options), ct);

    /// <inheritdoc />
    public Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig { Url = Url($"/media-library/media/{Enc(id)}"), Method = HttpMethod.Delete, ApiKey = options?.ApiKey }, ct);

    /// <inheritdoc />
    public Task SetTagsAsync(
        string id, IEnumerable<string> tags, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestVoidAsync(
            new HttpRequestConfig
            {
                Url = Url($"/media-library/media/{Enc(id)}/tags"),
                Method = HttpMethod.Post,
                // Swagger requires `id` inside SetMediaTagsCommand.
                Body = new { id, tags },
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
                Url = Url("/media-library/media/upload"),
                Method = HttpMethod.Post,
                RawContent = form,
                ApiKey = options?.ApiKey
            }, ct);
    }

}
