using AtlasCms.Sdk.Http;
using AtlasCms.Sdk.Modules;

namespace AtlasCms.Sdk;

public sealed class AtlasCmsClient
{
    public IContentsApi Contents { get; }
    public IMediaApi Media { get; }
    public IUsersApi Users { get; }
    public IRolesApi Roles { get; }
    public IModelsApi Models { get; }
    public IComponentsApi Components { get; }
    public IGraphqlApi Graphql { get; }

    private AtlasCmsClient(IContentsApi contents, IMediaApi media, IUsersApi users, IRolesApi roles, IModelsApi models, IComponentsApi components, IGraphqlApi graphql)
    {
        Contents = contents;
        Media = media;
        Users = users;
        Roles = roles;
        Models = models;
        Components = components;
        Graphql = graphql;
    }

    public static AtlasCmsClient Create(AtlasClientConfig config)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(config.ProjectId, nameof(config.ProjectId));
        ArgumentException.ThrowIfNullOrWhiteSpace(config.ApiKey, nameof(config.ApiKey));
#else
        if (string.IsNullOrWhiteSpace(config.ProjectId))
            throw new ArgumentException("ProjectId cannot be null or empty.", nameof(config.ProjectId));
        if (string.IsNullOrWhiteSpace(config.ApiKey))
            throw new ArgumentException("ApiKey cannot be null or empty.", nameof(config.ApiKey));
#endif

        var base_ = (config.BaseUrl ?? AtlasClientConfig.DefaultBaseUrl).TrimEnd('/');
        var restBaseUrl = $"{base_}/{Uri.EscapeDataString(config.ProjectId)}";
        var graphqlUrl = $"{restBaseUrl}/graphql";

        var http = new AtlasHttpClient(config);

        return new AtlasCmsClient(
            new ContentsApi(http, restBaseUrl),
            new MediaApi(http, restBaseUrl),
            new UsersApi(http, restBaseUrl),
            new RolesApi(http, restBaseUrl),
            new ModelsApi(http, restBaseUrl),
            new ComponentsApi(http, restBaseUrl),
            new GraphqlApi(http, graphqlUrl)
        );
    }
}
