using BasicTaskManagement.Core.Validation;

namespace BasicTaskManagement.Core.DTO;

public class CreateTaskGroupDTO
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsFavorite { get; init; }
    public ValidationResult Validate()
    {
        bool IsNameValid = !string.IsNullOrWhiteSpace(Name) && Name.Length > 0 && Name.Length <= 50;
        return new() { IsValid = IsNameValid, ErrorMessage = IsNameValid ? string.Empty : "Task group Name is required and must be 50 characters or fewer." };
    }

}
