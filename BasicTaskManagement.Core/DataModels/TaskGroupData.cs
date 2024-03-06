using BasicTaskManagement.Core.DTO;

namespace BasicTaskManagement.Core.DataModels;

public class TaskGroupData(int id, string name, bool isFavorite, IList<TaskItemDTO> items) : List<TaskItemDTO>(items)
{
    public int Id { get; init; } = id;
    public string Name { get; init; } = name;
    public bool IsFavorite { get; init; } = isFavorite;
}
