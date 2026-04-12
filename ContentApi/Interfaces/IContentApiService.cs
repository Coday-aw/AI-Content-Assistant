using K4U2.DTOs;
using K4U2.Models;

namespace K4U2.Interfaces;

public interface IContentApiService
{
   Task<ResponseDto> CreatePromptHistoryAsync(RequestDto dto); 
   Task<List<ResponseDto>> GetPromptHistoryAsync(string? category, DateTime? sort, DateTime? startDate);
   Task<bool> DeletePromptHistoryAsync(int id);
}