namespace BasicTaskManagement.Core.DTO;

public class TaskGroupDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required bool IsFavorite { get; init; }
    public required IEnumerable<TaskItemDTO> TaskItems { get; init; }

    public static TaskGroupDTO NotFound => new() { Id = 0, Name = "not found", IsFavorite = false, TaskItems = Enumerable.Empty<TaskItemDTO>() };
}
