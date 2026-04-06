using K4U2.Models;

namespace K4U2.Interfaces;

public interface IContentApiRepository
{
    Task<PromptHistory> CreatePromptHistoryAsync(PromptHistory promptHistory);
    Task<IQueryable<PromptHistory>> GetPromptHistoryAsync();
    Task<bool> DeletePromptHistoryAsync(int id);
}