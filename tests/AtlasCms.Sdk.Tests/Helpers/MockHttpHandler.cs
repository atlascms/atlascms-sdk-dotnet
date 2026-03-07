using System.Net;
using System.Text;

namespace AtlasCms.Sdk.Tests.Helpers;

public sealed class MockHttpHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;

    public HttpRequestMessage? LastRequest { get; private set; }
    public string? LastRequestBody { get; private set; }

    public MockHttpHandler(string responseJson, int statusCode = 200)
        : this(_ => new HttpResponseMessage((HttpStatusCode)statusCode)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        })
    { }

    public MockHttpHandler(Func<HttpRequestMessage, HttpResponseMessage> responder)
    {
        _responder = responder;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        LastRequest = request;
        if (request.Content is not null)
            LastRequestBody = await request.Content.ReadAsStringAsync(ct);
        return _responder(request);
    }

    public static AtlasCmsClient BuildClient(MockHttpHandler handler,
        string project = "my-project",
        string restBase = "https://api.example.com",
        string gqlBase = "https://graphql.example.com/graphql",
        string apiKey = "test-key")
        => AtlasCmsClient.Create(new AtlasClientConfig
        {
            Project = project,
            RestBaseUrl = restBase,
            GraphqlBaseUrl = gqlBase,
            ApiKey = apiKey,
            HttpClient = new HttpClient(handler)
        });
}
