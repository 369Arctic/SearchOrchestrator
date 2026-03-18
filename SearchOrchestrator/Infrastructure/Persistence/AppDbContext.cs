using Microsoft.EntityFrameworkCore;
using SearchOrchestrator.Domain;

namespace SearchOrchestrator.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<IndexingTask> Tasks => Set<IndexingTask>();
        public DbSet<Source> Sources => Set<Source>();
    }
}
