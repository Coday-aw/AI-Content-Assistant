using LLM_Api.DTOs;
using LLM_Api.Models;

namespace LLM_Api.Interfaces;

public interface IProxyApiService
{
    Task<ResponseDto> GetChatResponseAsync(string userMessage,  string apiKey);
}