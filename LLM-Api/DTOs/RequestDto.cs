namespace LLM_Api.DTOs;

public record RequestDto
{ 
    public string ModelName { get; set; } =  string.Empty;
    public int Temperature {get; set; }
    public int MaxTokens { get; set; }
}


