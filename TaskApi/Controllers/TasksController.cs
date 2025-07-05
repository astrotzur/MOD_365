using TaskApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly GraphServiceClient _graph;
    private readonly ILogger<TasksController> _logger;
    private readonly string _siteId;
    private readonly string _listId;

    private static string? TryGetFirstAssignedUserDisplayName(object assignedToRaw)
    {
        try
        {
            var json = (System.Text.Json.JsonElement)assignedToRaw;
            if (json.ValueKind == System.Text.Json.JsonValueKind.Array && json.GetArrayLength() > 0)
            {
                var firstUser = json.EnumerateArray().First();
                if (firstUser.TryGetProperty("displayName", out var nameProp))
                {
                    return nameProp.GetString();
                }
            }
            return "";
        }
        catch
        {
            return "";
        }
    }

    public TasksController(GraphServiceClient graph, ILogger<TasksController> logger, IConfiguration config)
    {
        _graph = graph;
        _logger = logger;
        _siteId = config["SharePoint:SiteId"] ?? throw new ArgumentNullException("SharePoint:SiteId missing in configuration");
        _listId = config["SharePoint:TasksListId"] ?? throw new ArgumentNullException("SharePoint:TasksListId missing in configuration");
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TaskDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("GET /api/tasks called");
        try
        {
            _logger.LogDebug("Fetching tasks from SharePoint list {ListId} on site {SiteId}", _listId, _siteId);
            var items = await _graph.Sites[_siteId]
            .Lists[_listId]
            .Items
            .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Expand = new[] { "fields" };
                });
            var result = new List<TaskDto>();

            if (items?.Value != null)
            {
                foreach (var item in items.Value)
                {
                    var fields = item.Fields?.AdditionalData;
                    var task = new TaskDto
                    {
                        Id = item.Id,
                        Title = fields != null && fields.ContainsKey(TaskListColumns.Title) ? fields[TaskListColumns.Title]?.ToString() : "",
                        Status = fields != null && fields.ContainsKey(TaskListColumns.Status) ? fields[TaskListColumns.Status]?.ToString() : "",
                        DueDate = fields != null && fields.ContainsKey(TaskListColumns.DueDate) ? fields[TaskListColumns.DueDate]?.ToString() : "",
                        AssignedTo = fields != null && fields.TryGetValue(TaskListColumns.AssignedTo, out var assignedToRaw) && assignedToRaw != null
                            ? TryGetFirstAssignedUserDisplayName(assignedToRaw)
                            : "",
                        Priority = fields != null && fields.ContainsKey(TaskListColumns.Priority) ? fields[TaskListColumns.Priority]?.ToString() : "",
                        Description = fields != null && fields.ContainsKey(TaskListColumns.Description) ? fields[TaskListColumns.Description]?.ToString() : ""
                    };

                    _logger.LogDebug("Mapped task {TaskId}: {Title}", task.Id, task.Title);
                    _logger.LogDebug("Raw AssignedTo JSON: {AssignedTo}",
                        fields != null && fields.ContainsKey(TaskListColumns.AssignedTo) ? fields[TaskListColumns.AssignedTo] : "null");
                    result.Add(task);
                }
            }
            else
            {
                _logger.LogWarning("No tasks found in list {ListId}", _listId);
            }
            return Ok(result);
        }
        catch (ServiceException ex)
        {
            var statusCode = ex.ResponseStatusCode > 0
                ? ex.ResponseStatusCode
                : 500;

            _logger.LogError(ex, "Error retrieving tasks: {Message}", ex.Message);

            return StatusCode(statusCode, new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTaskDto newTask)
    {
        _logger.LogInformation("POST /api/tasks called with Title={Title}", newTask.Title);

        try
        {
            // build the Graph item fields
            var fields = new Microsoft.Graph.Models.FieldValueSet
            {
                AdditionalData = new Dictionary<string, object?>
                {
                    ["Title"] = newTask.Title,
                    ["Status"] = newTask.Status ?? "Not Started",
                    ["DueDate"] = newTask.DueDate,
                    ["Priority"] = newTask.Priority ?? "Normal",
                    ["Description"] = newTask.Description ?? ""
                    // AssignedTo is more complex since it’s a user field, we can revisit that
                }
            };

            var listItem = new Microsoft.Graph.Models.ListItem
            {
                Fields = fields
            };

            var createdItem = await _graph.Sites[_siteId]
                .Lists[_listId]
                .Items
                .PostAsync(listItem);

            _logger.LogInformation("Created task with Id={Id}", createdItem?.Id);

            return CreatedAtAction(nameof(Get), new { id = createdItem?.Id }, createdItem);
        }
        catch (ServiceException ex)
        {
            var statusCode = ex.ResponseStatusCode > 0
                ? ex.ResponseStatusCode
                : 500;

            _logger.LogError(ex, "Error creating task: {Message}", ex.Message);

            return StatusCode(statusCode, new { error = ex.Message });
        }
    }

}
