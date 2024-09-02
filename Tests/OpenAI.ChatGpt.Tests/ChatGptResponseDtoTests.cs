using OpenAI.ChatGPT.Abstractions.Dtos;

namespace OpenAI.ChatGPT.Tests;

/// <summary>
/// Unit tests for the <see cref="ChatGptResponseDto"/> class.
/// </summary>
[TestClass]
public class ChatGptResponseDtoTests
{
    /// <summary>
    /// Tests that the <see cref="ChatGptResponseDto"/> correctly sets the response and exception properties when both are provided.
    /// </summary>
    [TestMethod]
    public void ChatGptResponseDto_ShouldSetResponseAndExceptionCorrectly_WhenBothAreProvided()
    {
        // Arrange
        var chatGptResponse = new ChatGptRootResponse();
        var exception = new Exception("Test exception");

        // Act
        var dto = new ChatGptResponseDto(chatGptResponse, exception);

        // Assert
        Assert.AreEqual(chatGptResponse, dto.Response, "The Response property should match the provided ChatGptRootResponse object.");
        Assert.AreEqual(exception, dto.Exception, "The Exception property should match the provided Exception object.");
        Assert.IsTrue(dto.IsError, "IsError should be true when both response and exception are provided.");
    }

    /// <summary>
    /// Tests that the <see cref="ChatGptResponseDto"/> indicates an error when the response is null and an exception is provided.
    /// </summary>
    [TestMethod]
    public void ChatGptResponseDto_ShouldIndicateError_WhenResponseIsNullAndExceptionIsProvided()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var dto = new ChatGptResponseDto(null, exception);

        // Assert
        Assert.IsNull(dto.Response, "Response should be null when not provided.");
        Assert.AreEqual(exception, dto.Exception, "Exception should match the provided exception.");
        Assert.IsTrue(dto.IsError, "IsError should be true when the response is null and an exception is provided.");
    }

    /// <summary>
    /// Tests that the <see cref="ChatGptResponseDto"/> indicates an error when both the response and exception are null.
    /// </summary>
    [TestMethod]
    public void ChatGptResponseDto_ShouldIndicateError_WhenResponseIsNullAndExceptionIsNotProvided()
    {
        // Arrange
        // No response and no exception provided

        // Act
        var dto = new ChatGptResponseDto(null);

        // Assert
        Assert.IsNull(dto.Response, "Response should be null when not provided.");
        Assert.IsNull(dto.Exception, "Exception should be null when not provided.");
        Assert.IsTrue(dto.IsError, "IsError should be true when both response and exception are null.");
    }

    /// <summary>
    /// Tests that the <see cref="ChatGptResponseDto"/> does not indicate an error when the response is provided and the exception is null.
    /// </summary>
    [TestMethod]
    public void ChatGptResponseDto_ShouldNotIndicateError_WhenResponseIsProvidedAndExceptionIsNull()
    {
        // Arrange
        var chatGptResponse = new ChatGptRootResponse();

        // Act
        var dto = new ChatGptResponseDto(chatGptResponse);

        // Assert
        Assert.AreEqual(chatGptResponse, dto.Response, "The Response property should match the provided ChatGptRootResponse object.");
        Assert.IsNull(dto.Exception, "Exception should be null when not provided.");
        Assert.IsFalse(dto.IsError, "IsError should be false when the response is provided and the exception is null.");
    }
}