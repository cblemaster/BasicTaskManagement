using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class CompletedTaskItemsPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private ReadOnlyCollection<TaskItemDTO> _taskItems = default!;

    private bool IsShowComplete { get; set; }

    [RelayCommand]
    private void PageAppearing()
    {
        IsShowComplete = false;
        LoadDataAsync();
    }

    [RelayCommand]
    private static void ItemsSelectionChanged() { }

    [RelayCommand]
    private void ShowCompletedFilterChanged()
    {
        IsShowComplete = !IsShowComplete;
        LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        TaskItems = (await _dataService.GetCompletedTaskItemsAsync()).ToList().AsReadOnly();
    }
}
