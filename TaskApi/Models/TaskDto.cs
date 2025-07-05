namespace TaskApi.Models
{
    /// <summary>
    /// Represents a task item returned from the SharePoint Tasks list.
    /// </summary>
    public class TaskDto
    {
        /// <summary>
        /// The unique identifier of the task in SharePoint.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// The title of the task.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// The current status of the task (e.g., Not Started, In Progress, Completed).
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// The due date of the task, in string format (ISO recommended).
        /// </summary>
        public string? DueDate { get; set; }

        /// <summary>
        /// The name of the person assigned to the task.
        /// </summary>
        public string? AssignedTo { get; set; }

        /// <summary>
        /// The priority of the task (e.g., High, Normal, Low).
        /// </summary>
        public string? Priority { get; set; }

        /// <summary>
        /// The HTML-formatted description of the task.
        /// </summary>
        public string? Description { get; set; }
    }
}
