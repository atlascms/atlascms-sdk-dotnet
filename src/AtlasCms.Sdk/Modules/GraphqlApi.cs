using AtlasCms.Sdk.Http;

namespace AtlasCms.Sdk.Modules;

/// <summary>
/// Provides implementation for GraphQL query execution in the Atlas CMS system.
/// This class handles all HTTP communication for GraphQL-related operations.
/// </summary>
internal sealed class GraphqlApi(AtlasHttpClient http, string graphqlBaseUrl) : IGraphqlApi
{
    /// <inheritdoc />
    public Task<GraphqlResponse<TData>> ExecuteAsync<TData>(
        GraphqlRequest<Dictionary<string, object?>> request, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => ExecuteAsync<TData, Dictionary<string, object?>>(request, options, ct);

    /// <inheritdoc />
    public Task<GraphqlResponse<TData>> ExecuteAsync<TData, TVariables>(
        GraphqlRequest<TVariables> request, AtlasRequestOptions? options = null, CancellationToken ct = default)
        => http.RequestAsync<GraphqlResponse<TData>>(
            new HttpRequestConfig
            {
                Url = graphqlBaseUrl,
                Method = HttpMethod.Post,
                Body = request,
                ApiKey = options?.ApiKey
            }, ct);
}
