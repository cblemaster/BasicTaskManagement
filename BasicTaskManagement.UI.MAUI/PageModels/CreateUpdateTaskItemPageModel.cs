using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using BasicTaskManagement.Core.Validation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class CreateUpdateTaskItemPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private CreateUpdateTaskItemDTO _taskItem = default;

    public int Id { get; set; }

    public bool OriginalIsComplete { get; set; }

    [ObservableProperty]
    private IReadOnlyCollection<TaskGroupDTO> _taskGroups = default;

    [ObservableProperty]
    private TaskGroupDTO selectedTaskGroup = default;

    [RelayCommand]
    private async Task PageAppearing()
    {
        if (Id > 0)
        {
            await LoadDataAsync();
            OriginalIsComplete = TaskItem.IsComplete;
        }
        else { TaskItem = new(); }

        await LoadTaskGroupsAsync();
        if (TaskGroups.SingleOrDefault(tg => tg.Id == TaskItem.TaskGroupId) is TaskGroupDTO dto)
        {
            SelectedTaskGroup = dto;
        }
    }

    [RelayCommand]
    private static async Task CancelClickedAsync() => await Shell.Current.Navigation.PopModalAsync();

    [RelayCommand]
    private async Task SaveClicked()
    {
        //if (OriginalIsComplete)
        //{
        //    await Shell.Current.DisplayAlert("Error!", "Completed task items cannot be updated.", "OK");
        //    return;
        //}
        
        ValidationResult validationResult = TaskItem.Validate();

        if (!validationResult.IsValid)
        {
            await Shell.Current.DisplayAlert("Error!", validationResult.ErrorMessage, "OK");
            return;
        }

        TaskItem.TaskGroupId = SelectedTaskGroup.Id;
        TaskItem.UpdateDate = DateTime.Today;
        if (TaskItem.IsComplete)
        {
            TaskItem.CompletedDate = DateTime.Today;
        }
        
        await _dataService.UpdateTaskItemAsync(Id, TaskItem);
        await Shell.Current.Navigation.PopModalAsync();
    }

    private async Task LoadDataAsync()
    {
        TaskItemDTO item = await _dataService.GetTaskItemAsync(Id);
        TaskItem = MapTaskItemDTOToCreateUpdateTaskItemDTO(item);
    }

    private async Task LoadTaskGroupsAsync()
    {
        List<TaskGroupDTO> groups = (await _dataService.GetTaskGroupsAsync(true)).ToList();
        groups.Insert(0, new() { Id = 0, Name = "not selected", IsFavorite = false, TaskItems = Enumerable.Empty<TaskItemDTO>() });

        TaskGroups = groups.AsReadOnly();
    }

    private static CreateUpdateTaskItemDTO MapTaskItemDTOToCreateUpdateTaskItemDTO(TaskItemDTO taskItemDTO) =>
        new()
        {
            Id = taskItemDTO.Id,
            Name = taskItemDTO.Name,
            Notes = taskItemDTO.Notes,
            IsImportant = taskItemDTO.IsImportant,
            IsComplete = taskItemDTO.IsComplete,
            DueDate = taskItemDTO.DueDate,
            CompletedDate = taskItemDTO.CompletedDate,
            CreateDate = taskItemDTO.CreateDate,
            UpdateDate = taskItemDTO.UpdateDate,
            TaskGroupId = taskItemDTO.TaskGroupId,
            TaskGroupName = taskItemDTO.TaskGroupName,
            TaskGroupIsFavorite = taskItemDTO.TaskGroupIsFavorite,
        };
}
