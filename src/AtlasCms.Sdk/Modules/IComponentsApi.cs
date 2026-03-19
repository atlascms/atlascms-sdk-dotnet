using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Input model for creating a component.
/// </summary>
public record CreateComponentInput
{
    public required string Key { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public IReadOnlyList<IField>? Attributes { get; init; }
}

/// <summary>
/// Input model for updating a component.
/// </summary>
public record UpdateComponentInput
{
    public required string Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public IReadOnlyList<IField>? Attributes { get; init; }
}

/// <summary>
/// Provides operations for managing components in Atlas CMS.
/// </summary>
public interface IComponentsApi
{
    /// <summary>
    /// Lists all components with optional query filtering.
    /// </summary>
    /// <param name="query">Optional query string for filtering and pagination.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    /// <returns>A paginated result containing the list of components.</returns>
    Task<PagedResult<Component>> ListAsync(
        string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a specific component by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the component.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    /// <returns>The component if found.</returns>
    Task<Component> GetByIdAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Creates a new component.
    /// </summary>
    /// <param name="payload">The component creation input.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    /// <returns>The result containing the ID of the created component.</returns>
    Task<CreateResult> CreateAsync(
        CreateComponentInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing component.
    /// </summary>
    /// <param name="payload">The component update input.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    Task UpdateAsync(
        UpdateComponentInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Deletes a component.
    /// </summary>
    /// <param name="id">The unique identifier of the component to delete.</param>
    /// <param name="options">Optional request options including custom API key.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default);
}
