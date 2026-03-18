namespace SearchOrchestrator.Application.DTO
{
    public class IndexingTaskResponseDto
    {
        public Guid TaskId { get; set; }
        public Domain.TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
