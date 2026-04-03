namespace LLM_Api.DTOs;

public record ResponseDto
{
  public string Message { get; set; } =  string.Empty;
  public string Model { get; set; } =  string.Empty;
  public int TokensUsed { get; set; }
}

