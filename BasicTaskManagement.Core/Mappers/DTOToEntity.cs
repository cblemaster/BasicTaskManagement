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
}
