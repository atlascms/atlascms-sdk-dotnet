using System.Text.Json.Nodes;
using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Input model for creating content.
/// </summary>
public record CreateContentInput
{
    /// <summary>
    /// The locale for the content. Optional if using default locale.
    /// </summary>
    public string? Locale { get; init; }

    /// <summary>
    /// Dynamic attributes for the content. Supports any JSON-serializable properties.
    /// </summary>
    public JsonObject? Attributes { get; init; }
}

/// <summary>
/// Input model for updating content.
/// </summary>
public record UpdateContentInput
{
    /// <summary>
    /// The locale for the content update.
    /// </summary>
    public string? Locale { get; init; }

    /// <summary>
    /// Dynamic attributes to update. Only specified properties will be updated.
    /// </summary>
    public JsonObject? Attributes { get; init; }
}

/// <summary>
/// Provides operations for managing content in Atlas CMS.
/// </summary>
public interface IContentsApi
{
    /// <summary>
    /// Lists content of a specific type with optional query filtering.
    /// </summary>
    /// <param name="type">The content type name.</param>
    /// <param name="query">Optional query string for filtering, sorting, and pagination.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    /// <returns>A paginated result containing the list of content.</returns>
    Task<PagedResult<Content>> ListAsync(
        string type, string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a specific content item by ID.
    /// </summary>
    /// <param name="type">The content type name.</param>
    /// <param name="id">The unique identifier of the content.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    /// <returns>The content item if found; throws an exception if not found.</returns>
    Task<Content> GetByIdAsync(
        string type, string id, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a singleton content item (unique content of a specific type).
    /// </summary>
    /// <param name="type">The content type name.</param>
    /// <param name="query">Optional query string for filtering.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    /// <returns>The singleton content item.</returns>
    Task<Content> GetSingleAsync(
        string type, string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Counts the total number of content items of a specific type.
    /// </summary>
    /// <param name="type">The content type name.</param>
    /// <param name="query">Optional query string for filtering.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    /// <returns>The total count of content items.</returns>
    Task<int> CountAsync(
        string type, string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Creates a new content item.
    /// </summary>
    /// <param name="type">The content type name.</param>
    /// <param name="payload">The content input data.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    /// <returns>The result containing the ID of the created content.</returns>
    Task<CreateResult> CreateAsync(
        string type, CreateContentInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing content item.
    /// </summary>
    /// <param name="type">The content type name.</param>
    /// <param name="id">The unique identifier of the content to update.</param>
    /// <param name="payload">The content update data.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    Task UpdateAsync(
        string type, string id, UpdateContentInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Deletes a content item.
    /// </summary>
    /// <param name="type">The content type name.</param>
    /// <param name="id">The unique identifier of the content to delete.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    Task RemoveAsync(
        string type, string id, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Changes the publication status of a content item.
    /// </summary>
    /// <param name="type">The content type name.</param>
    /// <param name="id">The unique identifier of the content.</param>
    /// <param name="status">The new publication status.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    Task ChangeStatusAsync(
        string type, string id, ContentStatus status, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Creates a new translation of existing content in a different locale.
    /// </summary>
    /// <param name="type">The content type name.</param>
    /// <param name="id">The unique identifier of the content to translate.</param>
    /// <param name="locale">The target locale for the translation.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    /// <returns>The result containing the ID of the new translation.</returns>
    Task<CreateResult> CreateTranslationAsync(
        string type, string id, string? locale = null, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Duplicates an existing content item.
    /// </summary>
    /// <param name="type">The content type name.</param>
    /// <param name="id">The unique identifier of the content to duplicate.</param>
    /// <param name="locales">Whether to duplicate all locale versions.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    /// <returns>The result containing the ID of the duplicated content.</returns>
    Task<CreateResult> DuplicateAsync(
        string type, string id, bool locales = false, AtlasRequestOptions? options = null, CancellationToken ct = default);
}
