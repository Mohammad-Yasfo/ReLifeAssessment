using Microsoft.Extensions.Options;
using OpenAI.ChatGPT.Abstractions.Contracts;
using OpenAI.ChatGPT.Core;
using OpenAI.ChatGPT.Options;

namespace OpenAI.ChatGPT.Extensions;

/// <summary>
/// Extension methods for setting up OpenAI ChatGPT services in the DI container.
/// </summary>
public static class IServiceCollectionExtension
{
    /// <summary>
    /// Registers the core dependencies for OpenAI ChatGPT services in the dependency injection container.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    /// <param name="configuration">The configuration containing ChatGPT options.</param>
    /// <returns>The IServiceCollection with the registered services.</returns>
    public static IServiceCollection AddCoreDI(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind ChatGPT options from configuration
        services.Configure<ChatGptOptions>(configuration.GetSection(ChatGptOptions.CONFIG_PATH));

        // Register the HttpClient for IChatGptService with configuration
        services.AddHttpClient<IChatGptService, DefaultChatGptService>((serviceProvider, httpClient) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<ChatGptOptions>>().Value;
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {options.ApiKey}");
        });

        return services;
    }
}