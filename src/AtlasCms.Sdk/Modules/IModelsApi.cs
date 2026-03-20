using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Input model for creating a content model.
/// </summary>
public record CreateModelInput
{
    public string? Key { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public bool? IsSingle { get; init; }
    public bool? Localizable { get; init; }
    public bool? EnableStageMode { get; init; }
    public bool? EnableSeo { get; init; }
    public IReadOnlyList<IField>? Attributes { get; init; }
    public ModelProperties? Properties { get; init; }
}

/// <summary>
/// Input model for updating a content model.
/// </summary>
public record UpdateModelInput
{
    public required string Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public bool? Localizable { get; init; }
    public bool? EnableStageMode { get; init; }
    public bool? EnableSeo { get; init; }
    public IReadOnlyList<IField>? Attributes { get; init; }
    public ModelProperties? Properties { get; init; }
}

/// <summary>
/// Provides operations for managing content models in Atlas CMS.
/// </summary>
public interface IModelsApi
{
    /// <summary>
    /// Lists all content models with optional query filtering.
    /// </summary>
    /// <param name="query">Optional query string for filtering and pagination.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    /// <returns>A paginated result containing the list of models.</returns>
    Task<IReadOnlyList<Model>> ListAsync(
        bool? system = null, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a specific model by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the model.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    /// <returns>The model if found.</returns>
    Task<Model> GetByIdAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Creates a new content model.
    /// </summary>
    /// <param name="payload">The model creation input.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    /// <returns>The result containing the ID of the created model.</returns>
    Task<KeyResult<string>> CreateAsync(
        CreateModelInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing content model.
    /// </summary>
    /// <param name="payload">The model update input.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    Task<KeyResult<string>> UpdateAsync(
        UpdateModelInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Deletes a content model.
    /// </summary>
    /// <param name="id">The unique identifier of the model to delete.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default);

}
