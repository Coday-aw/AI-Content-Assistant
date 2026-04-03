using LLM_Api.DTOs;
using LLM_Api.Interfaces;
using LLM_Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LLM_Api.Controllers;

[Controller]
[Route("api/llmProxy")]
public class ProxyApiController : ControllerBase
{
    private readonly IProxyApiService _proxyApiService;

    public ProxyApiController(IProxyApiService proxyApiService)
    {
        _proxyApiService = proxyApiService;
    }

    [HttpPost]
    public async Task<ActionResult<ResponseDto>> CreateGenerateMessage([FromBody] string message)
    {
        var response = await _proxyApiService.GetChatResponseAsync(message);
        return Ok(response);
    }
}