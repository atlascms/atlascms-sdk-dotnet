using AtlasCms.Sdk.Http;
using AtlasCms.Sdk.Modules;

namespace AtlasCms.Sdk;

public sealed class AtlasCmsClient
{
    public IContentsApi Contents { get; }
    public IMediaApi Media { get; }
    public IUsersApi Users { get; }
    public IRolesApi Roles { get; }
    public IGraphqlApi Graphql { get; }

    private AtlasCmsClient(IContentsApi contents, IMediaApi media, IUsersApi users, IRolesApi roles, IGraphqlApi graphql)
    {
        Contents = contents;
        Media = media;
        Users = users;
        Roles = roles;
        Graphql = graphql;
    }

    public static AtlasCmsClient Create(AtlasClientConfig config)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(config.Project, nameof(config.Project));
        ArgumentException.ThrowIfNullOrWhiteSpace(config.RestBaseUrl, nameof(config.RestBaseUrl));
        ArgumentException.ThrowIfNullOrWhiteSpace(config.GraphqlBaseUrl, nameof(config.GraphqlBaseUrl));
        ArgumentException.ThrowIfNullOrWhiteSpace(config.ApiKey, nameof(config.ApiKey));
#else
        if (string.IsNullOrWhiteSpace(config.Project))
            throw new ArgumentException("Project cannot be null or empty.", nameof(config.Project));
        if (string.IsNullOrWhiteSpace(config.RestBaseUrl))
            throw new ArgumentException("RestBaseUrl cannot be null or empty.", nameof(config.RestBaseUrl));
        if (string.IsNullOrWhiteSpace(config.GraphqlBaseUrl))
            throw new ArgumentException("GraphqlBaseUrl cannot be null or empty.", nameof(config.GraphqlBaseUrl));
        if (string.IsNullOrWhiteSpace(config.ApiKey))
            throw new ArgumentException("ApiKey cannot be null or empty.", nameof(config.ApiKey));
#endif

        var http = new AtlasHttpClient(config);

        return new AtlasCmsClient(
            new ContentsApi(http, config.RestBaseUrl, config.Project),
            new MediaApi(http, config.RestBaseUrl, config.Project),
            new UsersApi(http, config.RestBaseUrl, config.Project),
            new RolesApi(http, config.RestBaseUrl, config.Project),
            new GraphqlApi(http, config.GraphqlBaseUrl)
        );
    }
}
