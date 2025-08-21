namespace ShhhToshiApp.DTOs.TaskDTOs
{
    public class TaskItemDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int Points { get; init; }
        public bool IsActive { get; init; }
    }
}
