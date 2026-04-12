using K4U2.DTOs;
using K4U2.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace K4U2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContentApiController : ControllerBase
{
    private readonly IContentApiService _contentApiService;

    public ContentApiController(IContentApiService contentApiService)
    {
        _contentApiService = contentApiService;
    }

    [HttpPost]
    public async Task<ActionResult<ResponseDto>> CreatePromptHistoryAsync([FromBody] RequestDto dto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var response = await _contentApiService.CreatePromptHistoryAsync(dto);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ResponseDto>> GetPromptHistoryAsync([FromQuery]string? category, [FromBody] DateTime? sort, [FromQuery] DateTime? startDate)
    {
        var response = await _contentApiService.GetPromptHistoryAsync(category, sort, startDate);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePromptHistoryAsync(int id) 
    {
        await _contentApiService.DeletePromptHistoryAsync(id);
        return NoContent();
    }
    
}