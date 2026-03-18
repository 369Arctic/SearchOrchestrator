using SearchOrchestrator.Application.DTO;

namespace SearchOrchestrator.Application.Interfaces
{
    public interface IIndexingService
    {
        Task<IndexingTaskResponseDto> StartAsync(StartIndexingRequestDto request);
        Task<IndexingTaskResponseDto> GetStatusAsync(Guid taskId);
        Task<SearchResponseDto> SearchAsync(string query);
        Task ProcessAsync(CancellationToken ct);
    }
}
