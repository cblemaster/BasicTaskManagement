using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Entities;

namespace BasicTaskManagement.Core.Mappers;

public static class DTOToEntity
{
    public static TaskGroup MapCreateTaskGroup(CreateTaskGroupDTO dto) =>
        new()
        {
            Name = dto.Name,
            IsFavorite = dto.IsFavorite,
        };

    public static TaskItem MapCreateUpdateTaskItem(CreateUpdateTaskItemDTO createItem) =>
        new()
        {
            Id = createItem.Id,
            Name = createItem.Name,
            Notes = createItem.Notes,
            IsImportant = createItem.IsImportant,
            IsComplete = createItem.IsComplete,
            DueDate = createItem.DueDate,
            CompletedDate = createItem.CompletedDate,
            CreateDate = createItem.CreateDate,
            UpdateDate = createItem.UpdateDate,
            TaskGroupId = createItem.TaskGroupId,
        };
}
