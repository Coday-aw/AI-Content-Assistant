using K4U2.DTOs;
using K4U2.Exceptions;
using K4U2.Interfaces;
using K4U2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace K4U2.Services;

public class ContentApiService : IContentApiService
{
    private readonly IContentApiRepository _repository;
    private readonly HttpClient _httpClient;
    private readonly string? _secretKey;
    
    public ContentApiService(IContentApiRepository repository,  HttpClient httpClient, IConfiguration config)
    {
        _repository = repository;
        _httpClient = httpClient;
        _secretKey = config["ProxyApi:SecretKey"];
    }

    public async Task<List<ResponseDto>> GetPromptHistoryAsync(string ? category, DateTime? sort, DateTime? startDate)
    {
        // get prompt history from repository put them in query
        var query = await _repository.GetPromptHistoryAsync();
        
        // if we have category, get all propmt with that category and put it in query
        if(!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);
        
        if(sort != null)
            query = query.Where(p => p.CreatedAt  >= sort);
        
        if(startDate != null)
            query = query.Where(p => p.CreatedAt >= startDate);
        
        // return a dto response in a list. 
        return await query.Select(p => new ResponseDto(p.Id,p.Message, p.Response, p.Category, p.CreatedAt)).ToListAsync();
    }
    
    public async Task<ResponseDto> CreatePromptHistoryAsync(RequestDto dto)
    {
        // first get the prompt message from the respuest dto
        var message = dto.Message;
        // declare result outside try/catch to access in history prompt and return statement 
        LlmResponseDto? result;
        try
        {
            // send secret key in head 
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _secretKey);
            // post to the proxy api 
            var httpResponse = await _httpClient.PostAsJsonAsync("api/proxyllm",  message);
            // make sure the response is successful 
            httpResponse.EnsureSuccessStatusCode();
            // get the result 
            result = await httpResponse.Content.ReadFromJsonAsync<LlmResponseDto>();
            // make sure result is not null
        }
        catch (HttpRequestException e)
        { 
            throw new ProxyApiUnavailableException("Api is unavailable, please try again later", e);
        }
        
        if (result == null)
            throw new ProxyApiUnavailableException("Api returned empty content");
        
        // create new prompt history entity
        var newPromptHistory = new PromptHistory
        {
            Message = dto.Message,
            Response = result.Message,
            Category = dto.Category,
            CreatedAt = DateTime.Now
        };
        // send the new prompt histroy to database 
        var createdPromptHistory = await _repository.CreatePromptHistoryAsync(newPromptHistory);
        // return the response and send it make to user in dto format.
        return new ResponseDto(createdPromptHistory.Id,createdPromptHistory.Message, createdPromptHistory.Response, createdPromptHistory.Category,  createdPromptHistory.CreatedAt);
    }

    public async Task<bool> DeletePromptHistoryAsync(int id)
    {
        return await _repository.DeletePromptHistoryAsync(id);
    }
}