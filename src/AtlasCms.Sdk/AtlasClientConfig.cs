namespace AtlasCms.Sdk;

public class AtlasClientConfig
{
    public const string DefaultBaseUrl = "https://api.atlascms.io";

    public required string ProjectId { get; init; }
    public required string ApiKey { get; init; }
    /// <summary>Defaults to "https://api.atlascms.io".</summary>
    public string? BaseUrl { get; init; }
    /// <summary>Optional custom HttpClient for testing or advanced scenarios.</summary>
    public HttpClient? HttpClient { get; init; }
}
