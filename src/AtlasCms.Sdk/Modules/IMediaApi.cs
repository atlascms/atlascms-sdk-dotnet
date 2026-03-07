using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Represents the input data required to upload or update a media file in the Atlas CMS system.
/// </summary>
public record MediaUploadInput
{
    /// <summary>Gets or initializes the file stream to upload. This is required.</summary>
    public required Stream File { get; init; }

    /// <summary>Gets or initializes the folder path where the file should be stored in the media library.</summary>
    public string? Folder { get; init; }

    /// <summary>Gets or initializes the custom name for the uploaded file. If not provided, the original file name will be used.</summary>
    public string? FileName { get; init; }

    /// <summary>Gets or initializes the MIME content type of the file being uploaded.</summary>
    public string? ContentType { get; init; }

    /// <summary>Gets or initializes the ID of an existing media file to replace. If supplied, replaces the existing media with this ID and the folder parameter is ignored.</summary>
    public string? Id { get; init; }
}

/// <summary>
/// Provides access to media library management operations in the Atlas CMS system.
/// </summary>
public interface IMediaApi
{
    /// <summary>
    /// Retrieves a paginated list of media files from the library.
    /// </summary>
    /// <param name="query">Optional query string for filtering media by name, tags, or other searchable fields.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a paginated result of media files.</returns>
    Task<PagedResult<Media>> ListAsync(
        string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a specific media file by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the media file to retrieve.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the media file data.</returns>
    Task<Media> GetByIdAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Deletes a media file from the library.
    /// </summary>
    /// <param name="id">The unique identifier of the media file to delete.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Assigns or updates tags for a media file.
    /// </summary>
    /// <param name="id">The unique identifier of the media file to tag.</param>
    /// <param name="tags">The collection of tag strings to assign to the media file.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetTagsAsync(
        string id, IEnumerable<string> tags, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Uploads a new media file to the library or replaces an existing one.
    /// </summary>
    /// <param name="input">The input data containing the file stream and optional metadata.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the uploaded media file data.</returns>
    Task<Media> UploadAsync(
        MediaUploadInput input, AtlasRequestOptions? options = null, CancellationToken ct = default);
}
