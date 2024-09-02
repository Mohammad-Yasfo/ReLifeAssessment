namespace OpenAI.ChatGPT.Options;

/// <summary>
/// Represents the configuration options for the OpenAI ChatGPT service.
/// </summary>
public class ChatGptOptions
{
    /// <summary>
    /// The configuration path for binding ChatGPT options from the app settings.
    /// </summary>
    public const string CONFIG_PATH = "OpenAI:ChatGPT";

    /// <summary>
    /// The URL endpoint for the OpenAI ChatGPT completions API.
    /// </summary>
    public string CompletionsUrl { get; set; } = string.Empty;

    /// <summary>
    /// The API key used for authenticating requests to the OpenAI API.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// The model name to be used in requests to the OpenAI API (e.g., "gpt-3.5-turbo").
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// The role to be assigned for the messages (e.g., "user" or "system").
    /// </summary>
    public string Role { get; set; } = string.Empty;
}