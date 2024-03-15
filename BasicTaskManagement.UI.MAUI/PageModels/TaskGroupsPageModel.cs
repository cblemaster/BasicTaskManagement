using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using BasicTaskManagement.UI.MAUI.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class TaskGroupsPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private ReadOnlyCollection<TaskGroupSummaryDTO?> _taskGroups = default!;

    [ObservableProperty]
    private TaskGroupSummaryDTO selectedTaskGroup = default!;

    [RelayCommand]
    private async Task PageAppearingAsync() => await LoadDataAsync();

    [RelayCommand]
    private async Task GroupsSelectionChangedAsync() => await Shell.Current.Navigation.PushModalAsync(new TaskItemPage(SelectedTaskGroup.Id));

    [RelayCommand]
    private static async Task CreateGroupClickedAsync() => await Shell.Current.Navigation.PushModalAsync(new CreateUpdateTaskGroupPage(0));

    private async Task LoadDataAsync()
    {
        IEnumerable<TaskGroupSummaryDTO?> groups = await _dataService.GetTaskGroupsAsync();
        if (groups is not null)
        {
            TaskGroups = groups.ToList().AsReadOnly();
        }
        
    }
}
