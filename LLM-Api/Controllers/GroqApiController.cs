using LLM_Api.DTOs;
using LLM_Api.Interfaces;
using LLM_Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LLM_Api.Controllers;

[Controller]
[Route("api/llama")]
public class GroqApiController : ControllerBase
{
    private readonly IGroqApiService _groqApiService;

    public GroqApiController(IGroqApiService groqApiService)
    {
        _groqApiService = groqApiService;
    }

    [HttpPost]
    public async Task<ActionResult<ResponseDto>> CreateGenerateMessage([FromBody] string message)
    {
        var response = await _groqApiService.GetChatResponseAsync(message);
        return Ok(response);
    }
}