using SearchOrchestrator.Application.DTO;
using SearchOrchestrator.Application.Interfaces;
using SearchOrchestrator.Domain;
using SearchOrchestrator.Infrastructure.Search;

namespace SearchOrchestrator.Application.Services
{
    public class IndexingService : IIndexingService
    {
        private readonly ITaskRepository _taskRepo;
        private readonly ISearchEngineClient _client;
        private readonly ILogger<IndexingService> _logger;

        public IndexingService(ITaskRepository taskRepository, ISearchEngineClient client, ILogger<IndexingService> logger)
        {
            _taskRepo = taskRepository;
            _client = client;
            _logger = logger;
        }

        public async Task<IndexingTaskResponseDto?> GetStatusAsync(Guid taskId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);

            return task == null ? null : Map(task);
        }

        public async Task ProcessAsync(CancellationToken ct)
        {
            var tasks = await _taskRepo.GetPendingAsync();

            foreach (var task in tasks)
            {
                try
                {
                    task.Status = Domain.TaskStatus.InProgress;
                    task.StartedAt = DateTime.UtcNow;

                    for (int attempt = 0; attempt < 3; attempt++)
                    {
                        try
                        {
                            await _client.StartIndexingAsync(task.Source.Path, ct);

                            task.Status = Domain.TaskStatus.Completed;
                            task.FinishedAt = DateTime.UtcNow;
                            break;
                        }
                        catch (Exception ex)
                        {
                            task.RetryCount++;
                            _logger.LogWarning(ex, "Retry {Attempt} for task {TaskId}", attempt, task.Id);

                            if (attempt == 2)
                                throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    task.Status = Domain.TaskStatus.Failed;
                    task.Error = ex.Message;
                }

                await _taskRepo.SaveAsync();
            }
        }

        public async Task<SearchResponseDto> SearchAsync(string query)
        {
            var results = await _client.SearchAsync(query, CancellationToken.None);

            return new SearchResponseDto
            {
                Results = results,
            };
        }

        public async Task<IndexingTaskResponseDto> StartAsync(StartIndexingRequestDto request)
        {
            var existing = await _taskRepo.GetByIdempotencyKeyAsync(request.IdempotencyKey);
            if (existing != null)
                return Map(existing);

            var source = new Source
            {
                Id = Guid.NewGuid(),
                Path = request.SourcePath,
                CreatedAt = DateTime.UtcNow,
            };

            var task = new IndexingTask
            {
                Id = Guid.NewGuid(),
                Source = source,
                SourceId = source.Id,
                Status = Domain.TaskStatus.Pending,
                IdempotencyKey = request.IdempotencyKey,
                CreatedAt = DateTime.UtcNow
            };

            await _taskRepo.AddAsync(task);
            await _taskRepo.SaveAsync();

            return Map(task);
        }

        /// <summary>
        /// Доменная модель в dto
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private static IndexingTaskResponseDto Map(IndexingTask task)
        {
            return new()
            {
                TaskId = task.Id,
                Status = task.Status,
                CreatedAt = task.CreatedAt
            };
        }
    }
}
