using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.ChatGPT.Abstractions.Contracts;
using OpenAI.ChatGPT.Extensions;
using OpenAI.ChatGPT.Options;

namespace OpenAI.ChatGPT.Tests;

/// <summary>
/// Contains unit tests for the <see cref="IServiceCollectionExtension"/> class, specifically the <see cref="IServiceCollectionExtension.AddCoreDI"/> method.
/// </summary>
[TestClass]
public class IServiceCollectionExtensionTests
{
    /// <summary>
    /// Tests that the <see cref="IServiceCollectionExtension.AddCoreDI"/> method registers services correctly.
    /// </summary>
    [TestMethod]
    public void AddCoreDI_ShouldRegisterServicesCorrectly()
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();

        // Create a configuration with test data
        var keyValuePairs = new[]
        {
            new KeyValuePair<string, string?>($"{ChatGptOptions.CONFIG_PATH}:ApiKey", "test-api-key"),
            new KeyValuePair<string, string?>($"{ChatGptOptions.CONFIG_PATH}:CompletionsUrl", "https://api.openai.com/v1/completions"),
            new KeyValuePair<string, string?>($"{ChatGptOptions.CONFIG_PATH}:Model", "gpt-3.5-turbo"),
            new KeyValuePair<string, string?>($"{ChatGptOptions.CONFIG_PATH}:Role", "user")
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(keyValuePairs)
            .Build();

        // Act
        services.AddCoreDI(configuration);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        // Verify that ChatGptOptions is configured correctly
        var options = serviceProvider.GetRequiredService<IOptions<ChatGptOptions>>().Value;
        Assert.AreEqual("test-api-key", options.ApiKey, "ApiKey should be set to 'test-api-key'.");
        Assert.AreEqual("https://api.openai.com/v1/completions", options.CompletionsUrl, "CompletionsUrl should be set correctly.");
        Assert.AreEqual("gpt-3.5-turbo", options.Model, "Model should be set correctly.");
        Assert.AreEqual("user", options.Role, "Role should be set correctly.");

        // Verify that IChatGptService is registered and the HttpClient is configured
        var chatGptService = serviceProvider.GetRequiredService<IChatGptService>();
        Assert.IsNotNull(chatGptService, "IChatGptService should be registered.");

        // Verify that the HttpClient has the correct authorization header
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient(nameof(IChatGptService));
        Assert.IsNotNull(httpClient.DefaultRequestHeaders.Authorization, "Authorization header should not be null.");
        Assert.AreEqual("Bearer test-api-key", httpClient.DefaultRequestHeaders.Authorization.ToString(), "Authorization header should be set correctly.");
    }
}