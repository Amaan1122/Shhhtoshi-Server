namespace ShhhToshiApp.Models.TaskSystem
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public bool IsActive { get; set; }

    }
}
