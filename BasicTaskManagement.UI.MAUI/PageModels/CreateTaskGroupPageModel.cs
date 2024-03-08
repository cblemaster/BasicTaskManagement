using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using BasicTaskManagement.Core.Validation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class CreateTaskGroupPageModel : ObservableObject
{
    private readonly IDataService _dataService;

    public CreateTaskGroupPageModel(IDataService dataService)
    {
        _dataService = dataService;
        CreateGroup = new();
    }

    [ObservableProperty]
    private CreateTaskGroupDTO createGroup = default;

    [RelayCommand]
    private async Task CreateClickedAsync()
    {
        ValidationResult validationResult = CreateGroup.Validate();

        if (!validationResult.IsValid)
        {
            await Shell.Current.DisplayAlert("Error!", validationResult.ErrorMessage, "OK");
            return;
        }

        if (await _dataService.CreateTaskGroupAsync(CreateGroup) is not null)
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
        else
        {
            await Shell.Current.DisplayAlert("Error!", "Error creating task group.", "OK");
        }
    }

    [RelayCommand]
    private async static Task CancelClicked() => await Shell.Current.Navigation.PopModalAsync();
}
