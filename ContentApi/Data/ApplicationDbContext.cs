using K4U2.Models;
using Microsoft.EntityFrameworkCore;

namespace K4U2.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<PromptHistory> PromptHistories { get; set; }

}