using K4U2.Data;
using K4U2.Exceptions;
using K4U2.Interfaces;
using K4U2.Models;
using Microsoft.EntityFrameworkCore;

namespace K4U2.Repositories;

public class ContentApiRepository : IContentApiRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public ContentApiRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IQueryable<PromptHistory>> GetPromptHistoryAsync()
    {
       return await Task.FromResult(_dbContext.PromptHistories.AsQueryable());
    }

    public async Task<PromptHistory> CreatePromptHistoryAsync(PromptHistory promptHistory)
    {
        var newPrompt = await _dbContext.PromptHistories.AddAsync(promptHistory);
        await _dbContext.SaveChangesAsync();
        return newPrompt.Entity;
    }

    public async Task<bool> DeletePromptHistoryAsync(int id)
    {
        var prompt = await _dbContext.PromptHistories.FirstOrDefaultAsync(p => p.Id == id);

        if (prompt == null)
            throw new NotFoundException("Prompt not found");
        _dbContext.PromptHistories.Remove(prompt);
        
        await _dbContext.SaveChangesAsync();
        return true;
       
    }
}