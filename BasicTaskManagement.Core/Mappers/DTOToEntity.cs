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

    public static TaskItem MapCreateUpdateTaskItem(CreateUpdateTaskItemDTO dto) =>
        new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Notes = dto.Notes,
            IsImportant = dto.IsImportant,
            IsComplete = dto.IsComplete,
            DueDate = dto.DueDate,
            CompletedDate = dto.CompletedDate,
            CreateDate = dto.CreateDate,
            UpdateDate = dto.UpdateDate,
            TaskGroupId = dto.TaskGroupId,
        };
}
