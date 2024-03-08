using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class CreateUpdateTaskItemPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private CreateUpdateTaskItemDTO _taskItem = default;

    public int Id { get; set; }

    [ObservableProperty]
    private IReadOnlyCollection<TaskGroupDTO> _taskGroups = default;

    [ObservableProperty]
    private TaskGroupDTO selectedTaskGroup = default;

    [RelayCommand]
    private async Task PageAppearing()
    {
        if (Id > 0) { await LoadDataAsync(); }
        else { TaskItem = new(); }

        await LoadTaskGroupsAsync();
        if (TaskGroups.SingleOrDefault(tg => tg.Id == TaskItem.TaskGroupId) is TaskGroupDTO dto)
        {
            SelectedTaskGroup = dto;
        }
    }

    //[RelayCommand]
    //private static void CancelClicked() => Shell.Current.Navigation.PopModalAsync();

    //[RelayCommand]
    //private async Task DeleteClickedAsync()
    //{
    //    if (TaskItem is not null)
    //    {
    //        await _dataService.DeleteTaskItem(Id);
    //        await Shell.Current.Navigation.PopModalAsync();
    //        await Shell.Current.Navigation.PopModalAsync();
    //    }
    //}

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
