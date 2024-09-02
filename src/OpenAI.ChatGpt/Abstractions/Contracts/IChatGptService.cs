using OpenAI.ChatGPT.Abstractions.Dtos;

namespace OpenAI.ChatGPT.Abstractions.Contracts;

/// <summary>
/// Defines the contract for a service that interacts with OpenAI's ChatGPT.
/// </summary>
public interface IChatGptService
{
    /// <summary>
    /// Asynchronously gets a response from the ChatGPT model based on the provided prompt.
    /// </summary>
    /// <param name="prompt">The input prompt to be sent to the ChatGPT model.</param>
    /// <returns>A task that represents the asynchronous operation, containing the response from ChatGPT.</returns>
    ValueTask<ChatGptResponseDto> GetChatGptResponseAsync(string prompt);
}