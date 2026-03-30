namespace LLM_Api.Models;

public class OpenAiResponse
{
    public Choice[]  choices { get; set; }
    public Usage  usage { get; set; }
}

public class Choice
{
   public Message  message { get; set; } 
}

public class Message
{
    public string content { get; set; }
    public string role { get; set; }
}

public class Usage
{
    public int prompt_tokens { get; set; }
    public int completion_tokens { get; set; }
    public int total_tokens { get; set; }
}
