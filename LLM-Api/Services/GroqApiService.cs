using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LLM_Api.DTOs;
using LLM_Api.Interfaces;
using LLM_Api.Models;

namespace LLM_Api.Services;

public class GroqApiService : IGroqApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _modelName;

    public GroqApiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["Llama:ApiKey"];
        _modelName = config["Llama:ModelName"];
    }

    public async Task<ResponseDto> GetChatResponseAsync(string userMessage)
    {
        var requestBody = new
        {
            model = _modelName,
            temperature = 1.0,
            max_tokens = 200,
            messages = new[]
            {
                new { role = "user", content = userMessage }
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.groq.com/openai/v1/chat/completions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"groq error: {response.StatusCode} - {error}");
        }
         
        var json = await response.Content.ReadAsStringAsync();
        
        var result = JsonSerializer.Deserialize<OpenAiResponse>(json);
        
        if (result == null)
        {
            throw new Exception("Error getting chat");
        }

        return new ResponseDto
        {
            Message = result.choices[0].message.content,
            Model = _modelName,
            TokensUsed = result.usage.total_tokens
        };
    }
}