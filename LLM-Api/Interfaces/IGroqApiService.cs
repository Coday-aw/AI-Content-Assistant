using LLM_Api.DTOs;
using LLM_Api.Models;

namespace LLM_Api.Interfaces;

public interface IGroqApiService
{
    Task<ResponseDto> GetChatResponseAsync(string userMessage);
}