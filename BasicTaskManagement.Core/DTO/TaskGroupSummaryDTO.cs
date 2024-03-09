namespace BasicTaskManagement.Core.DTO;

public class TaskGroupSummaryDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required bool IsFavorite { get; init; }
    public required int CountOfTaskItems { get; init; }
}
