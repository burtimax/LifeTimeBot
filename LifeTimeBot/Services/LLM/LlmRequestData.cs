using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class LlmRequestData
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("messages")]
    public List<LlmMessageItem> Messages { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }

    [JsonPropertyName("top_p")]
    public double TopP { get; set; }
}

public class LlmMessageItem
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}