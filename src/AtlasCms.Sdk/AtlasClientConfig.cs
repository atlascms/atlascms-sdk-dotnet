namespace AtlasCms.Sdk;

public class AtlasClientConfig
{
    public required string Project { get; init; }
    public required string RestBaseUrl { get; init; }
    public required string GraphqlBaseUrl { get; init; }
    public required string ApiKey { get; init; }
    /// <summary>Optional custom HttpClient for testing or advanced scenarios.</summary>
    public HttpClient? HttpClient { get; init; }
}
