using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using BasicTaskManagement.UI.MAUI.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class TaskGroupPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private TaskGroupDTO _taskGroup = default!;

    [ObservableProperty]
    private IReadOnlyCollection<TaskItemDTO> allTaskItems = default!;

    [ObservableProperty]
    private IReadOnlyCollection<TaskItemDTO> filteredTaskItems = default!;

    [ObservableProperty]
    private TaskItemDTO selectedTaskItem = default!;

    public int Id { get; set; }

    private bool IsShowComplete { get; set; }

    [RelayCommand]
    private async Task PageAppearingAsync() => await LoadDataAsync();

    [RelayCommand]
    private async Task ItemsSelectionChangedAsync() => await Shell.Current.Navigation.PushModalAsync(new TaskItemPage(SelectedTaskItem.Id));

    [RelayCommand]
    private async Task ShowCompletedFilterChangedAsync()
    {
        IsShowComplete = !IsShowComplete;
        await LoadDataAsync();
    }

    [RelayCommand]
    private async static Task CloseClickedAsync() => await Shell.Current.Navigation.PopModalAsync();

    [RelayCommand]
    private async Task UpdateTaskGroupClickedAsync() => await Shell.Current.Navigation.PushModalAsync(new CreateUpdateTaskGroupPage(Id));

    [RelayCommand]
    private async Task DeleteTaskGroupClickedAsync() => await Shell.Current.Navigation.PushModalAsync(new DeleteTaskGroupPage(Id));

    [RelayCommand]
    private static async Task CreateTaskItemClickedAsync() => await Shell.Current.Navigation.PushModalAsync(new CreateUpdateTaskItemPage(0));

    private async Task LoadDataAsync()
    {
        TaskGroup = (await _dataService.GetTaskGroupAsync(Id));
        AllTaskItems = TaskGroup.TaskItems.ToList().AsReadOnly();

        FilteredTaskItems = IsShowComplete ? AllTaskItems.ToList().AsReadOnly() : AllTaskItems.Where(ti => !ti.IsComplete).ToList().AsReadOnly();
    }
}
