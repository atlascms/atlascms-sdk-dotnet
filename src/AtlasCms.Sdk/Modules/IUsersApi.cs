using System.Text.Json.Nodes;
using AtlasCms.Sdk.Types;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Represents the input data required to create a new user in the Atlas CMS system.
/// </summary>
public record CreateUserInput
{
    /// <summary>Gets or initializes the first name of the user.</summary>
    public string? FirstName { get; init; }

    /// <summary>Gets or initializes the last name of the user.</summary>
    public string? LastName { get; init; }

    /// <summary>Gets or initializes the unique username of the user.</summary>
    public string? Username { get; init; }

    /// <summary>Gets or initializes the email address of the user.</summary>
    public string? Email { get; init; }

    /// <summary>Gets or initializes the mobile phone number of the user.</summary>
    public string? MobilePhone { get; init; }

    /// <summary>Gets or initializes the list of role identifiers to assign to the user.</summary>
    public IList<string>? Roles { get; init; }

    /// <summary>Gets or initializes the initial password for the user account.</summary>
    public string? Password { get; init; }

    /// <summary>Gets or initializes a value indicating whether the user account is active. Defaults to true.</summary>
    public bool IsActive { get; init; } = true;

    /// <summary>Gets or initializes the custom attributes associated with the user as a JSON object.</summary>
    public JsonObject? Attributes { get; init; }
}

/// <summary>
/// Represents the input data required to update an existing user in the Atlas CMS system.
/// </summary>
public record UpdateUserInput
{
    /// <summary>Gets or initializes the first name of the user.</summary>
    public string? FirstName { get; init; }

    /// <summary>Gets or initializes the last name of the user.</summary>
    public string? LastName { get; init; }

    /// <summary>Gets or initializes the unique username of the user.</summary>
    public string? Username { get; init; }

    /// <summary>Gets or initializes the email address of the user.</summary>
    public string? Email { get; init; }

    /// <summary>Gets or initializes the mobile phone number of the user.</summary>
    public string? MobilePhone { get; init; }

    /// <summary>Gets or initializes the list of role identifiers to assign to the user.</summary>
    public IList<string>? Roles { get; init; }

    /// <summary>Gets or initializes additional notes about the user.</summary>
    public string? Notes { get; init; }

    /// <summary>Gets or initializes the custom attributes associated with the user as a JSON object.</summary>
    public JsonObject? Attributes { get; init; }
}

/// <summary>
/// Provides access to user management operations in the Atlas CMS system.
/// </summary>
public interface IUsersApi
{
    /// <summary>
    /// Retrieves a paginated list of users from the system.
    /// </summary>
    /// <param name="query">Optional query string for filtering users by name, email, or other searchable fields.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a paginated result of users.</returns>
    Task<PagedResult<User>> ListAsync(
        string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Counts the total number of users in the system based on an optional query filter.
    /// </summary>
    /// <param name="query">Optional query string for filtering users by name, email, or other searchable fields.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the count of users.</returns>
    Task<int> CountAsync(
        string? query = null, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a specific user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user data.</returns>
    Task<User> GetByIdAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Creates a new user account in the system.
    /// </summary>
    /// <param name="payload">The input data containing the user information to create.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the creation result with the new user ID.</returns>
    Task<KeyResult<string>> CreateAsync(
        CreateUserInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing user account with new information.
    /// </summary>
    /// <param name="id">The unique identifier of the user to update.</param>
    /// <param name="payload">The input data containing the updated user information.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(
        string id, UpdateUserInput payload, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Deletes a user account from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveAsync(
        string id, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Changes the active status of a user account.
    /// </summary>
    /// <param name="id">The unique identifier of the user whose status should be changed.</param>
    /// <param name="isActive">A value indicating whether the user account should be active or inactive.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ChangeStatusAsync(
        string id, bool isActive, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Changes the password of a user account.
    /// </summary>
    /// <param name="id">The unique identifier of the user whose password should be changed.</param>
    /// <param name="password">The new password to set for the user account.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ChangePasswordAsync(
        string id, string password, AtlasRequestOptions? options = null, CancellationToken ct = default);
}
