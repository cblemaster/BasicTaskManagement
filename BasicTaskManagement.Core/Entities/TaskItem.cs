namespace BasicTaskManagement.Core.Entities;

public partial class TaskItem
{
    public int Id { get; set; }

    public int TaskGroupId { get; set; }

    public string Name { get; set; } = null!;

    public string? Notes { get; set; }

    public bool IsImportant { get; set; }

    public bool IsComplete { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual TaskGroup TaskGroup { get; set; } = null!;
}
