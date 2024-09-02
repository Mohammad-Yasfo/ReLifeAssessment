using OpenAI.ChatGPT.Options;

namespace OpenAI.ChatGPT.Tests;

/// <summary>
/// Unit tests for the <see cref="ChatGptOptions"/> class.
/// </summary>
[TestClass]
public class ChatGptOptionsTests
{
    /// <summary>
    /// Initializes resources before each test runs.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        // Setup code before each test if needed
    }

    /// <summary>
    /// Cleans up resources after each test runs.
    /// </summary>
    [TestCleanup]
    public void Cleanup()
    {
        // Cleanup code after each test if needed
    }

    /// <summary>
    /// Tests the default values of the <see cref="ChatGptOptions"/> class.
    /// </summary>
    [TestMethod]
    public void ChatGptOptionsTest()
    {
        // Arrange
        var chatGptOptions = new ChatGptOptions();

        // Act & Assert
        Assert.AreEqual("OpenAI:ChatGPT", ChatGptOptions.CONFIG_PATH, "CONFIG_PATH should match the expected constant value.");
        Assert.AreEqual(string.Empty, chatGptOptions.CompletionsUrl, "CompletionsUrl should be an empty string by default.");
        Assert.AreEqual(string.Empty, chatGptOptions.ApiKey, "ApiKey should be an empty string by default.");
        Assert.AreEqual(string.Empty, chatGptOptions.Model, "Model should be an empty string by default.");
        Assert.AreEqual(string.Empty, chatGptOptions.Role, "Role should be an empty string by default.");
    }
}