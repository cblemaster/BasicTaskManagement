using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class DeleteTaskGroupPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private TaskGroupDTO _taskGroup = default;

    public int Id { get; set; }

    [RelayCommand]
    private async Task PageAppearingAsync() => await LoadDataAsync();

    [RelayCommand]
    private static async Task CancelClickedAsync() => await Shell.Current.Navigation.PopModalAsync();

    [RelayCommand]
    private async Task DeleteClickedAsync()
    {
        if (TaskGroup is not null)
        {
            if (TaskGroup.TaskItems.Any())
            {
                await Shell.Current.DisplayAlert("Error!", "Cannot delete task group since it contains task items.", "OK");
                return;
            }

            await _dataService.DeleteTaskGroupAsync(Id);
            await Shell.Current.Navigation.PopModalAsync();
            await Shell.Current.Navigation.PopModalAsync();
        }
    }

    private async Task LoadDataAsync() => TaskGroup = (await _dataService.GetTaskGroupAsync(Id));
}