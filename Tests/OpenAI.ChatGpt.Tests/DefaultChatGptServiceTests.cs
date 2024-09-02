using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using OpenAI.ChatGPT.Abstractions.Dtos;
using OpenAI.ChatGPT.Core;
using OpenAI.ChatGPT.Options;
using System.Net;
using System.Text.Json;

namespace OpenAI.ChatGPT.Tests;

/// <summary>
/// Unit tests for the <see cref="DefaultChatGptService"/> class.
/// </summary>
[TestClass]
public class DefaultChatGptServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new(); // Mock for HttpClient handler
    private readonly Mock<IOptions<ChatGptOptions>> _optionsMock = new(); // Mock for ChatGPT options
    private readonly Mock<ILogger<DefaultChatGptService>> _loggerMock = new(); // Mock for logger
    private HttpClient _httpClient = null!;
    private DefaultChatGptService _service = null!;

    /// <summary>
    /// Initializes the test setup for each test case.
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
        // Mock the options with example values
        _optionsMock.Setup(o => o.Value).Returns(new ChatGptOptions
        {
            Model = "gpt-3.5-turbo",
            Role = "user",
            CompletionsUrl = "https://api.openai.com/v1/completions",
            ApiKey = "testing-key"
        });

        // Create HttpClient with mocked handler
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

        // Initialize the service with mocked dependencies
        _service = new DefaultChatGptService(_httpClient, _optionsMock.Object, _loggerMock.Object);
    }

    /// <summary>
    /// Tests that <see cref="DefaultChatGptService.GetChatGptResponseAsync"/> returns a valid response when the API call is successful.
    /// </summary>
    [TestMethod]
    public async Task GetChatGptResponseAsync_ShouldReturnResponse_WhenApiCallIsSuccessful()
    {
        // Arrange
        var prompt = "What is the capital of France?";
        var apiResponse = new ChatGptRootResponse
        {
            Choices = new List<Choice> { new Choice { Text = "Paris" } }
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(apiResponse))
            });

        // Act
        var result = await _service.GetChatGptResponseAsync(prompt);

        // Assert
        Assert.IsNotNull(result, "The result should not be null.");
        Assert.IsNotNull(result.Response, "The response should not be null.");
        Assert.AreEqual("Paris", result.Response.Choices[0].Text, "The text of the first choice should be 'Paris'.");
    }

    /// <summary>
    /// Tests that <see cref="DefaultChatGptService.GetChatGptResponseAsync"/> logs an error and returns an error response when the API call fails.
    /// </summary>
    [TestMethod]
    public async Task GetChatGptResponseAsync_ShouldLogError_WhenApiCallFails()
    {
        // Arrange
        var prompt = "What is the capital of France?";
        var exceptionMessage = "Network error";

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException(exceptionMessage));

        // Act
        var result = await _service.GetChatGptResponseAsync(prompt);

        // Assert
        Assert.IsNull(result.Response, "The response should be null when an exception is thrown.");
        Assert.IsNotNull(result.Exception, "The exception should not be null.");
        Assert.AreEqual(exceptionMessage, result.Exception.Message, "The exception message should match the expected message.");
        _loggerMock.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "Error while getting data from ChatGPT"),
                It.Is<Exception>(ex => ex.Message == exceptionMessage),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once,
            "The logger should log an error once with the correct message and exception.");
    }
}