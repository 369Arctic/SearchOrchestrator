namespace SearchOrchestrator.Domain
{
    /// <summary>
    /// Статусы задачи для индексации.
    /// </summary>
    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed,
        Failed,
        Cancelled
    }
}
