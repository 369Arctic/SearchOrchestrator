namespace SearchOrchestrator.Application.DTO
{
    public class StartIndexingRequestDto
    {
        public string SourcePath { get; set; } = null!;
        public string IdempotencyKey { get; set; } = null!;
    }
}
