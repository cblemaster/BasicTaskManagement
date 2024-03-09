using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Entities;

namespace BasicTaskManagement.Core.Mappers;

public static class DTOToEntity
{
    public static TaskGroup MapCreateTaskGroup(CreateTaskGroupDTO dto) =>
        new()
        {
            Id = dto.Id,
            Name = dto.Name,
            IsFavorite = dto.IsFavorite,
        };

    public static TaskGroup MapUpdateTaskGroup(CreateTaskGroupDTO dto, TaskGroup entity)
    {
        entity.Name = dto.Name;
        entity.IsFavorite = dto.IsFavorite;
        entity.TaskItems = entity.TaskItems;

        return entity;
    }

    public static TaskItem MapCreateUpdateTaskItem(CreateUpdateTaskItemDTO dto, TaskItem entity)
    {
        entity.Name = dto.Name;
        entity.Notes = dto.Notes;
        entity.IsImportant = dto.IsImportant;
        entity.IsComplete = dto.IsComplete;
        entity.TaskGroupId = dto.TaskGroupId;
        entity.DueDate = dto.DueDate;
        entity.UpdateDate = dto.UpdateDate;
        entity.CompletedDate = dto.CompletedDate;
        entity.CreateDate = dto.CreateDate;
        
        return entity;
    }
}
