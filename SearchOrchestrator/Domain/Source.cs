namespace SearchOrchestrator.Domain
{
    /// <summary>
    /// Источник данных для индексации.
    /// </summary>
    public class Source
    {
        public Guid Id { get; set; }
        public string Path { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
