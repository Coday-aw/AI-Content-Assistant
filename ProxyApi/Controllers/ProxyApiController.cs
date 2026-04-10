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
    private readonly string? _secretKey;

    public ProxyApiController(IProxyApiService proxyApiService, IConfiguration config)
    {
        _proxyApiService = proxyApiService;
        _secretKey = config["ProxyApi:SecretKey"];
    }

    [HttpPost]
    public async Task<ActionResult<ResponseDto>> CreateGenerateMessage([FromBody] string message)
    {
        var apiKey = Request.Headers["ApiKey"].FirstOrDefault();
        if (string.IsNullOrEmpty(apiKey))
            return Unauthorized("Api key is missing");
        var secretKey = _secretKey;
        
        if(apiKey != secretKey)
            return Unauthorized("Invalid api key");
        
        var response = await _proxyApiService.GetChatResponseAsync(message);
        return Ok(response);
    }
}