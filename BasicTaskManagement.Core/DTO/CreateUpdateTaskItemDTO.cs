using BasicTaskManagement.Core.Validation;
using System.Text;

namespace BasicTaskManagement.Core.DTO;

public class CreateUpdateTaskItemDTO
{
    public int Id { get; set; }
    public int TaskGroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public bool IsImportant { get; set; }
    public bool IsComplete { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string TaskGroupName { get; set; } = string.Empty;
    public bool TaskGroupIsFavorite { get; set; }

    public ValidationResult Validate()
    {
        bool taskGroupIdIsValid = TaskGroupId > 0;
        bool NameIsValid = !string.IsNullOrWhiteSpace(Name) && Name.Length > 1 && Name.Length <= 50;
        bool NotesIsValid = Notes is null || Notes.Length <= 100;
        bool allIsValid = taskGroupIdIsValid && NameIsValid && NotesIsValid;

        if (allIsValid) { return new() { IsValid = true, ErrorMessage = string.Empty }; }

        StringBuilder sb = new();
        if (!taskGroupIdIsValid) { sb.AppendLine("Invalid task group id."); }
        if (!NameIsValid) { sb.AppendLine("Name is required and must be 50 characters or fewer."); }
        if (!NotesIsValid) { sb.AppendLine("If Notes are provided, they must be 100 or fewer characters."); }

        return new() { IsValid = false, ErrorMessage = sb.ToString() };
    }
}
