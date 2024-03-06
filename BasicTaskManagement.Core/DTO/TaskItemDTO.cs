namespace BasicTaskManagement.Core.DTO;

public class TaskItemDTO
{
    public required int Id { get; init; }
    public required int TaskGroupId { get; init; }
    public required string Name { get; init; }
    public string? Notes { get; init; }
    public required bool IsImportant { get; init; }
    public required bool IsComplete { get; init; }
    public DateTime? DueDate { get; init; }
    public DateTime? CompletedDate { get; init; }
    public required DateTime CreateDate { get; init; }
    public DateTime? UpdateDate { get; init; }
    public required string TaskGroupName { get; init; }
    public required bool TaskGroupIsFavorite { get; init; }
}
