using Microsoft.EntityFrameworkCore;
using SearchOrchestrator.Application.Interfaces;
using SearchOrchestrator.Domain;

namespace SearchOrchestrator.Infrastructure.Persistence
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _db;
        public TaskRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<IndexingTask?> GetByIdAsync(Guid taskId)
        {
            return _db.Tasks.Include(u => u.Source)
                .FirstOrDefaultAsync(u => u.Id == taskId);
        }

        public Task<IndexingTask?> GetByIdempotencyKeyAsync(string key)
        {
            return _db.Tasks.Include(u => u.Source)
                .FirstOrDefaultAsync(u => u.IdempotencyKey == key);
        }

        public Task<List<IndexingTask>> GetPendingAsync()
        {
            return _db.Tasks.Include(u => u.Source)
                .Where(u => u.Status == Domain.TaskStatus.Pending)
                .ToListAsync();
        }

        public async Task AddAsync(IndexingTask task)
        {
            await _db.Tasks.AddAsync(task);
        }

        public Task SaveAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
