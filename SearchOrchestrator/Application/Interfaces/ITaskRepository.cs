using SearchOrchestrator.Domain;

namespace SearchOrchestrator.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<IndexingTask?> GetByIdAsync(Guid taskId);
        Task<IndexingTask?> GetByIdempotencyKeyAsync(string key);
        Task<List<IndexingTask>> GetPendingAsync();
        Task AddAsync(IndexingTask task);
        Task SaveAsync();
    }
}
