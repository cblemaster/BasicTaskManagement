using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using BasicTaskManagement.UI.MAUI.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class TaskItemsDueTodayPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private ReadOnlyCollection<TaskItemDTO> _taskItems = default!;

    [ObservableProperty]
    private TaskItemDTO selectedTaskItem = default;

    private bool IsShowComplete { get; set; }

    [RelayCommand]
    private void PageAppearing()
    {
        IsShowComplete = false;
        LoadDataAsync();
    }

    [RelayCommand]
    private void ItemsSelectionChanged() => Shell.Current.Navigation.PushModalAsync(new TaskItemPage(SelectedTaskItem.Id));

    [RelayCommand]
    private void ShowCompletedFilterChanged()
    {
        IsShowComplete = !IsShowComplete;
        LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        TaskItems = (await _dataService.GetTaskItemsDueTodayAsync(IsShowComplete)).ToList().AsReadOnly();
    }    
}
