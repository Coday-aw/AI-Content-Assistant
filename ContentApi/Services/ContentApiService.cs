using K4U2.DTOs;
using K4U2.Exceptions;
using K4U2.Interfaces;
using K4U2.Models;
using Microsoft.EntityFrameworkCore;

namespace K4U2.Services;

public class ContentApiService : IContentApiService
{
    private readonly IContentApiRepository _repository;
    private readonly HttpClient _httpClient;
    
    public ContentApiService(IContentApiRepository repository,  HttpClient httpClient)
    {
        _repository = repository;
        _httpClient = httpClient;
    }

    public async Task<List<ResponseDto>> GetPromptHistoryAsync(string ? category)
    {
        // get prompt history from repository put them in query
        var query = await _repository.GetPromptHistoryAsync();
        
        // if we have category, get all propmt with that category and put it in query
        if(!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);
        // return a dto response in a list. 
        return query.Select(p => new ResponseDto(p.Message, p.Response, p.Category, p.CreatedAt)).ToList();
    }
    
    public async Task<ResponseDto> CreatePromptHistoryAsync(RequestDto dto)
    {
        // first get the prompt message from the respuest dto
        var message = dto.Message;
        // post to the proxy api 
        var httpResponse = await _httpClient.PostAsJsonAsync("",  message);
        // make sure the response is successful 
        httpResponse.EnsureSuccessStatusCode();
        // get the result 
        var result = await httpResponse.Content.ReadFromJsonAsync<LlmResponseDto>();
        // make sure result is not null
        if (result == null)
         throw new ProxyApiUnavailableException("Api is not available", null);
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
        return new ResponseDto(createdPromptHistory.Message, createdPromptHistory.Response, createdPromptHistory.Category,  createdPromptHistory.CreatedAt);
    }

    public async Task<bool> DeletePromptHistoryAsync(int id)
    {
        return await _repository.DeletePromptHistoryAsync(id);
    }


}