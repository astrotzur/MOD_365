namespace TaskApi.Models
{
    /// <summary>
    /// Represents a new task to be created.
    /// </summary>
    public class CreateTaskDto
    {
        /// <summary>
        /// The title of the task.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The status of the task (e.g. Not Started, In Progress, Completed).
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// The due date in ISO 8601 format, e.g. "2025-08-01" or "2025-08-01T15:00:00Z".
        /// </summary>
        public string? DueDate { get; set; }

        /// <summary>
        /// The user assigned to the task (user email or loginName).
        /// </summary>
        public string? AssignedTo { get; set; }

        /// <summary>
        /// The priority of the task (e.g. Low, Normal, High).
        /// </summary>
        public string? Priority { get; set; }

        /// <summary>
        /// The HTML-rich description of the task.
        /// </summary>
        public string? Description { get; set; }
    }
}
