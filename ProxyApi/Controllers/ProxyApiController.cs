using LLM_Api.DTOs;
using LLM_Api.Interfaces;
using LLM_Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LLM_Api.Controllers;

[Route("api/proxyllm")]
[ApiController]
public class ProxyApiController : ControllerBase
{
    private readonly IProxyApiService _proxyApiService;
    
    public ProxyApiController(IProxyApiService proxyApiService, IConfiguration config)
    {
        _proxyApiService = proxyApiService;
    }

    [HttpPost]
    public async Task<ActionResult<ResponseDto>> CreateGenerateMessage([FromBody] string message)
    {
        var apiKey = Request.Headers["ApiKey"].FirstOrDefault();
        
        var response = await _proxyApiService.GetChatResponseAsync(message, apiKey);
        return Ok(response);
    }
}