using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LLM_Api.DTOs;
using LLM_Api.Interfaces;
using LLM_Api.Models;

namespace LLM_Api.Services;

public class ProxyApiService : IProxyApiService
{
    private readonly HttpClient _httpClient;
    private readonly string? _apiKey;
    private readonly string? _modelName;
    private readonly string? _secretKey;

    public ProxyApiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["ProxyApi:ApiKey"];
        _modelName = config["ProxyApi:ModelName"];
        _secretKey = config["ProxyApi:SecretKey"];
    }

    public async Task<ResponseDto> GetChatResponseAsync(string userMessage,  string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey))
            throw new ArgumentNullException(nameof(apiKey), "API Key cannot be null or empty");
        var secretKey = _secretKey;
        
        if(apiKey != secretKey)
            throw new ArgumentException("invalid API Key");
        
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
        
        var result = JsonSerializer.Deserialize<ProxyApiResponse>(json);
        
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