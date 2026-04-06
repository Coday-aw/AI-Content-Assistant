using K4U2.DTOs;
using K4U2.Models;

namespace K4U2.Interfaces;

public interface IContentApiService
{
   Task<ResponseDto> CreatePromptHistoryAsync(RequestDto dto); 
   Task<List<ResponseDto>> GetPromptHistoryAsync(string? category);
   Task<bool> DeletePromptHistoryAsync(int id);
}