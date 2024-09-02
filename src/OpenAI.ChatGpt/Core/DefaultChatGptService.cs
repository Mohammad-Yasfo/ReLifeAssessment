using Microsoft.Extensions.Options;
using OpenAI.ChatGPT.Abstractions.Contracts;
using OpenAI.ChatGPT.Abstractions.Dtos;
using OpenAI.ChatGPT.Options;

namespace OpenAI.ChatGPT.Core;

/// <summary>
/// Default implementation of the IChatGptService interface for interacting with OpenAI's ChatGPT.
/// </summary>
public class DefaultChatGptService : IChatGptService
{
    private readonly HttpClient _httpClient;
    private readonly ChatGptOptions _options;
    private readonly ILogger<DefaultChatGptService>? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultChatGptService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to send requests to the ChatGPT API.</param>
    /// <param name="options">The configuration options for ChatGPT, including model and endpoint settings.</param>
    /// <param name="logger">The logger instance for logging errors or informational messages.</param>
    public DefaultChatGptService(HttpClient httpClient, IOptions<ChatGptOptions> options, ILogger<DefaultChatGptService>? logger = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
    }

    /// <summary>
    /// Asynchronously gets a response from the ChatGPT model based on the provided prompt.
    /// </summary>
    /// <param name="prompt">The input prompt to be sent to the ChatGPT model.</param>
    /// <returns>A task that represents the asynchronous operation, containing the response from ChatGPT.</returns>
    public async ValueTask<ChatGptResponseDto> GetChatGptResponseAsync(string prompt)
    {
        // Prepare the request body with the model and prompt details.
        var requestBody = new
        {
            model = _options.Model,
            messages = new[]
            {
                new { role = _options.Role, content = prompt }
            }
        };

        try
        {
            // Send the POST request to the OpenAI ChatGPT endpoint.
            var response = await _httpClient.PostAsJsonAsync(_options.CompletionsUrl, requestBody);
            response.EnsureSuccessStatusCode();

            // Deserialize the response content into the ChatGptRootResponse object.
            var result = await response.Content.ReadFromJsonAsync<ChatGptRootResponse>();
            return new ChatGptResponseDto(result);
        }
        catch (Exception ex)
        {
            // Log the error if an exception occurs.
            _logger?.LogError(ex, "Error while getting data from ChatGPT");
            return new ChatGptResponseDto(null, ex);
        }
    }
}