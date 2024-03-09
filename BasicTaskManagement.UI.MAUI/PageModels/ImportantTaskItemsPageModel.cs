using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using BasicTaskManagement.UI.MAUI.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class ImportantTaskItemsPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private ReadOnlyCollection<TaskItemDTO> _taskItems = default;

    [ObservableProperty]
    private TaskItemDTO selectedTaskItem = default;

    private bool IsShowComplete { get; set; }

    [RelayCommand]
    private async Task PageAppearing() => await LoadDataAsync();

    [RelayCommand]
    private async Task ItemsSelectionChanged() => await Shell.Current.Navigation.PushModalAsync(new TaskItemPage(SelectedTaskItem.Id));

    [RelayCommand]
    private async Task ShowCompletedFilterChanged()
    {
        IsShowComplete = !IsShowComplete;
        await LoadDataAsync();
    }

    [RelayCommand]
    private static async Task CreateTaskItemClicked() => await Shell.Current.Navigation.PushModalAsync(new CreateUpdateTaskItemPage(0));

    private async Task LoadDataAsync() => TaskItems = (await _dataService.GetImportantTaskItemsAsync(IsShowComplete)).ToList().AsReadOnly();
}
