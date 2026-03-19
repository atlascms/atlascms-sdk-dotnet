using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasCms.Sdk.AspnetCore;

/// <summary>
/// Extension methods for registering the Atlas CMS SDK with the ASP.NET Core dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the Atlas CMS client with the service collection using configuration.
    /// </summary>
    /// <param name="services">The service collection to register with.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="sectionName">The configuration section name. Defaults to "AtlasCms".</param>
    /// <returns>The service collection for chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the configuration section is missing or incomplete.</exception>
    public static IServiceCollection AddAtlasCms(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = "AtlasCms")
    {
        var section = configuration.GetSection(sectionName);

        var projectId = section["ProjectId"];
        var apiKey = section["ApiKey"];
        var baseUrl = section["BaseUrl"];

        if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException(
                $"Atlas CMS configuration section '{sectionName}' is missing or incomplete. " +
                "Please ensure ProjectId and ApiKey are configured.");
        }

        return AddAtlasCms(services, new AtlasClientConfig
        {
            ProjectId = projectId,
            ApiKey = apiKey,
            BaseUrl = string.IsNullOrEmpty(baseUrl) ? null : baseUrl
        });
    }

    /// <summary>
    /// Registers the Atlas CMS client with the service collection using explicit configuration.
    /// </summary>
    /// <param name="services">The service collection to register with.</param>
    /// <param name="config">The Atlas CMS client configuration.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddAtlasCms(
        this IServiceCollection services,
        AtlasClientConfig config)
    {
        if (config == null)
            throw new ArgumentNullException(nameof(config));

        services.AddSingleton(config);
        services.AddSingleton(AtlasCmsClient.Create(config));

        return services;
    }

    /// <summary>
    /// Registers the Atlas CMS client with the service collection using a configuration delegate.
    /// </summary>
    /// <param name="services">The service collection to register with.</param>
    /// <param name="configureOptions">A delegate to configure the Atlas CMS client.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddAtlasCms(
        this IServiceCollection services,
        Action<AtlasClientConfigBuilder> configureOptions)
    {
        if (configureOptions == null)
            throw new ArgumentNullException(nameof(configureOptions));

        var builder = new AtlasClientConfigBuilder();
        configureOptions(builder);
        var config = builder.Build();

        return AddAtlasCms(services, config);
    }
}
