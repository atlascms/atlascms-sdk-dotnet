using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Represents the input data required to create a new role in the Atlas CMS system.
/// </summary>
public record CreateRoleInput
{
    /// <summary>Gets or initializes the name of the role. This is required.</summary>
    public required string Name { get; init; }

    /// <summary>Gets or initializes the list of permission identifiers to assign to the role.</summary>
    public IList<string>? Permissions { get; init; }
}

/// <summary>
/// Represents the input data required to update an existing role in the Atlas CMS system.
/// </summary>
public record UpdateRoleInput
{
    /// <summary>Gets or initializes the name of the role. This is required.</summary>
    public required string Name { get; init; }

    /// <summary>Gets or initializes the list of permission identifiers to assign to the role.</summary>
    public IList<string>? Permissions { get; init; }
}

/// <summary>
/// Provides access to role management operations in the Atlas CMS system.
/// </summary>
public interface IRolesApi
{
    /// <summary>
    /// Retrieves all available roles in the system.
    /// </summary>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all roles.</returns>
    Task<IReadOnlyList<Role>> ListAsync(
        AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Creates a new role in the system.
    /// </summary>
    /// <param name="payload">The input data containing the role information to create.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the creation result with the new role ID.</returns>
    Task<KeyResult<string>> CreateAsync(
        CreateRoleInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing role with new information.
    /// </summary>
    /// <param name="id">The unique identifier of the role to update.</param>
    /// <param name="payload">The input data containing the updated role information.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(
        string id, UpdateRoleInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Deletes a role from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the role to delete.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves all available permissions grouped by category.
    /// </summary>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of permission groups.</returns>
    Task<IReadOnlyList<PermissionGroup>> GetPermissionsAsync(
        AtlasRequestOptions? options = null, CancellationToken ct = default);
}
