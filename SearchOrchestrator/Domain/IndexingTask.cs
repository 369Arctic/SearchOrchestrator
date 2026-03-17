namespace SearchOrchestrator.Domain
{
    public class IndexingTask
    {
        public Guid Id { get; set; }
        public Guid SourceId { get; set; }
        public Source Source { get; set; } = null!;
        public TaskStatus Status { get; set; }
        public string IdempotencyKey { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public string? Error { get; set; }
        public int RetryCount { get; set; }

    }
}
