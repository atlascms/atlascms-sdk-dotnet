using System.Text.Json.Nodes;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Represents a GraphQL request with optional variables and operation name.
/// </summary>
/// <typeparam name="TVariables">The type of variables used in the GraphQL query.</typeparam>
public record GraphqlRequest<TVariables>
{
    /// <summary>Gets or initializes the GraphQL query string. This is required.</summary>
    public required string Query { get; init; }

    /// <summary>Gets or initializes the variables to be passed to the GraphQL query.</summary>
    public TVariables? Variables { get; init; }

    /// <summary>Gets or initializes the name of the GraphQL operation to execute, useful for multi-operation documents.</summary>
    public string? OperationName { get; init; }
}

/// <summary>
/// Represents an error returned by a GraphQL query execution.
/// </summary>
public record GraphqlError
{
    /// <summary>Gets or initializes the error message. This is required.</summary>
    public required string Message { get; init; }

    /// <summary>Gets or initializes the path to the field in the query that caused the error.</summary>
    public JsonArray? Path { get; init; }

    /// <summary>Gets or initializes additional error context and details as a JSON object.</summary>
    public JsonObject? Extensions { get; init; }
}

/// <summary>
/// Represents the response from a GraphQL query execution.
/// </summary>
/// <typeparam name="TData">The type of data returned by the GraphQL query.</typeparam>
public record GraphqlResponse<TData>
{
    /// <summary>Gets or initializes the data returned by the GraphQL query.</summary>
    public TData? Data { get; init; }

    /// <summary>Gets or initializes the list of errors that occurred during query execution, if any.</summary>
    public IReadOnlyList<GraphqlError>? Errors { get; init; }
}

/// <summary>
/// Provides access to GraphQL query execution in the Atlas CMS system.
/// </summary>
public interface IGraphqlApi
{
    /// <summary>
    /// Executes a GraphQL query with inline variables as a dictionary.
    /// </summary>
    /// <typeparam name="TData">The type of data expected in the response.</typeparam>
    /// <param name="request">The GraphQL request containing the query and variables.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the GraphQL response with data and any errors.</returns>
    Task<GraphqlResponse<TData>> ExecuteAsync<TData>(
        GraphqlRequest<Dictionary<string, object?>> request, AtlasRequestOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Executes a GraphQL query with strongly-typed variables.
    /// </summary>
    /// <typeparam name="TData">The type of data expected in the response.</typeparam>
    /// <typeparam name="TVariables">The type of variables used in the query.</typeparam>
    /// <param name="request">The GraphQL request containing the query and strongly-typed variables.</param>
    /// <param name="options">Optional request options including custom API key override.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the GraphQL response with data and any errors.</returns>
    Task<GraphqlResponse<TData>> ExecuteAsync<TData, TVariables>(
        GraphqlRequest<TVariables> request, AtlasRequestOptions? options = null, CancellationToken ct = default);
}
