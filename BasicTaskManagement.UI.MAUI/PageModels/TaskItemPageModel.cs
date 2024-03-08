using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using BasicTaskManagement.UI.MAUI.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class TaskItemPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private TaskItemDTO _taskItem = default;

    public int Id { get; set; }

    [RelayCommand]
    private void PageAppearing() => LoadDataAsync();

    [RelayCommand]
    private async Task CloseClicked() => await Shell.Current.Navigation.PopModalAsync();

    [RelayCommand]
    private async Task UpdateClicked() => await Shell.Current.Navigation.PushModalAsync(new CreateUpdateTaskItemPage(Id));

    [RelayCommand]
    private async Task DeleteClicked() => await Shell.Current.Navigation.PushModalAsync(new DeleteTaskItemPage(Id));

    private async Task LoadDataAsync() => TaskItem = (await _dataService.GetTaskItemAsync(Id));
}
