namespace ToDo.Models.Db
{
    /// <summary>
    ///     Task state
    /// </summary>
    public class TaskStatus
    {
        /// <summary>
        ///     Task unique identifier
        /// </summary>
        public int TaskStatusId { get; set; }

        /// <summary>
        ///     Display name for task state
        /// </summary>
        public string DisplayName { get; set; }

        public static TaskStatus NotStarted => new TaskStatus { TaskStatusId = 1, DisplayName = "Not started" };
        public static TaskStatus InProgress => new TaskStatus { TaskStatusId = 2, DisplayName = "In progress" };
        public static TaskStatus Completed => new TaskStatus { TaskStatusId = 3, DisplayName = "Completed" };
    }
}
