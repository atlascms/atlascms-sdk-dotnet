namespace AtlasCms.Sdk.AspnetCore;

/// <summary>
/// A fluent builder for configuring the Atlas CMS client.
/// </summary>
public class AtlasClientConfigBuilder
{
    private string? _project;
    private string? _restBaseUrl;
    private string? _graphqlBaseUrl;
    private string? _apiKey;
    private HttpClient? _httpClient;

    /// <summary>
    /// Sets the Atlas CMS project name.
    /// </summary>
    public AtlasClientConfigBuilder WithProject(string project)
    {
        if (string.IsNullOrWhiteSpace(project))
            throw new ArgumentException("Project cannot be null or empty.", nameof(project));
        _project = project;
        return this;
    }

    /// <summary>
    /// Sets the REST API base URL.
    /// </summary>
    public AtlasClientConfigBuilder WithRestBaseUrl(string restBaseUrl)
    {
        if (string.IsNullOrWhiteSpace(restBaseUrl))
            throw new ArgumentException("RestBaseUrl cannot be null or empty.", nameof(restBaseUrl));
        _restBaseUrl = restBaseUrl;
        return this;
    }

    /// <summary>
    /// Sets the GraphQL API base URL.
    /// </summary>
    public AtlasClientConfigBuilder WithGraphqlBaseUrl(string graphqlBaseUrl)
    {
        if (string.IsNullOrWhiteSpace(graphqlBaseUrl))
            throw new ArgumentException("GraphqlBaseUrl cannot be null or empty.", nameof(graphqlBaseUrl));
        _graphqlBaseUrl = graphqlBaseUrl;
        return this;
    }

    /// <summary>
    /// Sets the API key for authentication.
    /// </summary>
    public AtlasClientConfigBuilder WithApiKey(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("ApiKey cannot be null or empty.", nameof(apiKey));
        _apiKey = apiKey;
        return this;
    }

    /// <summary>
    /// Sets a custom HttpClient instance for advanced scenarios.
    /// </summary>
    public AtlasClientConfigBuilder WithHttpClient(HttpClient httpClient)
    {
        if (httpClient == null)
            throw new ArgumentNullException(nameof(httpClient));
        _httpClient = httpClient;
        return this;
    }

    /// <summary>
    /// Builds the Atlas CMS client configuration.
    /// </summary>
    /// <returns>The configured AtlasClientConfig.</returns>
    /// <exception cref="InvalidOperationException">Thrown when required configuration is missing.</exception>
    public AtlasClientConfig Build()
    {
        if (string.IsNullOrEmpty(_project))
            throw new InvalidOperationException("Project is required. Use WithProject() to set it.");
        if (string.IsNullOrEmpty(_restBaseUrl))
            throw new InvalidOperationException("RestBaseUrl is required. Use WithRestBaseUrl() to set it.");
        if (string.IsNullOrEmpty(_graphqlBaseUrl))
            throw new InvalidOperationException("GraphqlBaseUrl is required. Use WithGraphqlBaseUrl() to set it.");
        if (string.IsNullOrEmpty(_apiKey))
            throw new InvalidOperationException("ApiKey is required. Use WithApiKey() to set it.");

        return new AtlasClientConfig
        {
            Project = _project,
            RestBaseUrl = _restBaseUrl,
            GraphqlBaseUrl = _graphqlBaseUrl,
            ApiKey = _apiKey,
            HttpClient = _httpClient
        };
    }
}
