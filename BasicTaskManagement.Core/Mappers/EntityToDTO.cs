using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Entities;

namespace BasicTaskManagement.Core.Mappers;

public static class EntityToDTO
{
    public static TaskItemDTO MapTaskItem(TaskItem entity) =>
        new()
        {
            Id = entity.Id,
            TaskGroupId = entity.TaskGroupId,
            Name = entity.Name,
            Notes = entity.Notes,
            IsImportant = entity.IsImportant,
            IsComplete = entity.IsComplete,
            DueDate = entity.DueDate,
            CompletedDate = entity.CompletedDate,
            CreateDate = entity.CreateDate,
            UpdateDate = entity.UpdateDate,
            TaskGroupName = entity.TaskGroup.Name,
            TaskGroupIsFavorite = entity.TaskGroup.IsFavorite,
        };

    public static IEnumerable<TaskItemDTO> MapTaskItemCollection(IEnumerable<TaskItem> entities)
    {
        List<TaskItemDTO> collectionToReturn = [];

        foreach (TaskItem entity in entities)
        {
            TaskItemDTO dto = MapTaskItem(entity);
            collectionToReturn.Add(dto);
        }

        return collectionToReturn.AsEnumerable();
    }

    public static TaskGroupDTO MapTaskGroup(TaskGroup entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsFavorite = entity.IsFavorite,
            TaskItems = MapTaskItemCollection(entity.TaskItems),
        };

    public static TaskGroupSummaryDTO MapTaskGroupSummary(TaskGroup entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsFavorite = entity.IsFavorite,
            CountOfTaskItems = entity.TaskItems.Count,
        };

    public static IEnumerable<TaskGroupSummaryDTO> MapTaskGroupSummaryCollection(IEnumerable<TaskGroup> entities)
    {
        List<TaskGroupSummaryDTO> collectionToReturn = [];

        foreach (TaskGroup entity in entities)
        {
            TaskGroupSummaryDTO dto = MapTaskGroupSummary(entity);
            collectionToReturn.Add(dto);
        }

        return collectionToReturn.AsEnumerable();
    }
}
