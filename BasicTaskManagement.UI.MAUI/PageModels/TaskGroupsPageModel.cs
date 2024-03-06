using BasicTaskManagement.Core.DataModels;
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
    private ReadOnlyCollection<TaskGroupData> _taskGroups = default!;

    private bool IsShowComplete { get; set; }

    [RelayCommand]
    private void PageAppearing() => LoadDataAsync();

    [RelayCommand]
    private static void GroupsSelectionChanged() { }

    [RelayCommand]
    private static void CreateGroupClicked() => Shell.Current.Navigation.PushModalAsync(new CreateTaskGroupPage());

    [RelayCommand]
    private void ShowCompletedFilterChanged()
    {
        IsShowComplete = !IsShowComplete;
        LoadDataAsync();
    }

    private async Task LoadDataAsync() => TaskGroups = await _dataService.GetTaskGroupsAsync(IsShowComplete);
}
