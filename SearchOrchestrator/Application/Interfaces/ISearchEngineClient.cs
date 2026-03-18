
namespace SearchOrchestrator.Application.Interfaces
{
    // TODO пока чистая заглушка.
    public interface ISearchEngineClient
    {
        Task<List<string>> SearchAsync(string query, CancellationToken ct);
        Task StartIndexingAsync(string path, CancellationToken ct);
    }
}