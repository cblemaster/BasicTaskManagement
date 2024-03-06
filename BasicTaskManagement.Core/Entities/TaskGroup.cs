namespace BasicTaskManagement.Core.Entities;

public partial class TaskGroup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsFavorite { get; set; }

    public virtual ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
}
