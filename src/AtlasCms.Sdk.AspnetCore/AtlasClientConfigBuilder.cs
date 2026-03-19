namespace AtlasCms.Sdk.AspnetCore;

/// <summary>
/// A fluent builder for configuring the Atlas CMS client.
/// </summary>
public class AtlasClientConfigBuilder
{
    private string? _projectId;
    private string? _baseUrl;
    private string? _apiKey;
    private HttpClient? _httpClient;

    /// <summary>
    /// Sets the Atlas CMS project ID.
    /// </summary>
    public AtlasClientConfigBuilder WithProjectId(string projectId)
    {
        if (string.IsNullOrWhiteSpace(projectId))
            throw new ArgumentException("ProjectId cannot be null or empty.", nameof(projectId));
        _projectId = projectId;
        return this;
    }

    /// <summary>
    /// Sets the API base URL. Defaults to "https://api.atlascms.io".
    /// </summary>
    public AtlasClientConfigBuilder WithBaseUrl(string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentException("BaseUrl cannot be null or empty.", nameof(baseUrl));
        _baseUrl = baseUrl;
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
    /// <exception cref="InvalidOperationException">Thrown when required configuration is missing.</exception>
    public AtlasClientConfig Build()
    {
        if (string.IsNullOrEmpty(_projectId))
            throw new InvalidOperationException("ProjectId is required. Use WithProjectId() to set it.");
        if (string.IsNullOrEmpty(_apiKey))
            throw new InvalidOperationException("ApiKey is required. Use WithApiKey() to set it.");

        return new AtlasClientConfig
        {
            ProjectId = _projectId,
            ApiKey = _apiKey,
            BaseUrl = _baseUrl,
            HttpClient = _httpClient
        };
    }
}
