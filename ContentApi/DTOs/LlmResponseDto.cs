namespace K4U2.DTOs;

public record LlmResponseDto
{
    public string Message { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int MaxTokens { get; set; }
}