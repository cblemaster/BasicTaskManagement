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
    private TaskGroupDTO _taskGroup = default;

    public int Id { get; set; }

    [RelayCommand]
    private async Task PageAppearingAsync() => await LoadDataAsync();

    [RelayCommand]
    private async static Task CloseClickedAsync() => await Shell.Current.Navigation.PopModalAsync();

    [RelayCommand]
    private async Task UpdateClickedAsync() => await Shell.Current.Navigation.PushModalAsync(new CreateTaskGroupPage(Id));

    [RelayCommand]
    private async Task DeleteClickedAsync() => await Shell.Current.Navigation.PushModalAsync(new DeleteTaskGroupPage(Id));

    private async Task LoadDataAsync() => TaskGroup = (await _dataService.GetTaskGroupAsync(Id));
}
