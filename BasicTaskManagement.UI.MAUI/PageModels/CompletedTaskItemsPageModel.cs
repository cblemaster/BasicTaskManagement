using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using BasicTaskManagement.UI.MAUI.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class CompletedTaskItemsPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private ReadOnlyCollection<TaskItemDTO> _taskItems = default;

    [ObservableProperty]
    private TaskItemDTO selectedTaskItem = default;

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
    private static async Task CreateTaskItemClickedAsync() => await Shell.Current.Navigation.PushModalAsync(new CreateUpdateTaskItemPage(0));

    private async Task LoadDataAsync() => TaskItems = (await _dataService.GetCompletedTaskItemsAsync()).ToList().AsReadOnly();
}
