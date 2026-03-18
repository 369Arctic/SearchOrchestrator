
namespace SearchOrchestrator.Infrastructure.Search
{
    public class FakeSearchEngineClient : ISearchEngineClient
    {
        public async Task<List<string>> SearchAsync(string query, CancellationToken ct)
        {
            await Task.Delay(300, ct);
            return new List<string> { query };
        }

        public async Task StartIndexingAsync(string path, CancellationToken ct)
        {
            await Task.Delay(500, ct);

            if (Random.Shared.Next(0, 5) == 0)
                throw new InvalidOperationException("Random error from search engine");
        }
    }
}
