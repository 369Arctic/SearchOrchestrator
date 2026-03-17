namespace SearchOrchestrator.Application.DTO
{
    public class IndexingTaskResponseDto
    {
        public Guid TaskId { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime CreeatedAt { get; set; }
    }
}
