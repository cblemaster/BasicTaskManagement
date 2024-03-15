using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using BasicTaskManagement.Core.Validation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class CreateUpdateTaskGroupPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private CreateTaskGroupDTO createGroup = default!;

    public int Id { get; set; }

    [RelayCommand]
    private async Task PageAppearingAsync()
    {
        if (Id > 0)
        {
            await LoadDataAsync();
        }
        else { CreateGroup = new(); }
    }

    [RelayCommand]
    private static async Task CancelClickedAsync() => await Shell.Current.Navigation.PopModalAsync();

    [RelayCommand]
    private async Task SaveClickedAsync()
    {
        foreach (TaskGroupSummaryDTO? group in await _dataService.GetTaskGroupsAsync())
        {
            if (group is not null && group.Id != CreateGroup.Id && group.Name == CreateGroup.Name)
            {
                await Shell.Current.DisplayAlert("Error!", $"There is already a task group named {CreateGroup.Name}", "OK");
                return;
            }
        }

        ValidationResult validationResult = CreateGroup.Validate();

        if (!validationResult.IsValid)
        {
            await Shell.Current.DisplayAlert("Error!", validationResult.ErrorMessage, "OK");
            return;
        }

        if (Id > 0)
        {
            await _dataService.UpdateTaskGroupAsync(Id, CreateGroup);
            await Shell.Current.Navigation.PopModalAsync();
        }
        else
        {
            if (await _dataService.CreateTaskGroupAsync(CreateGroup) is not null)
            {
                await Shell.Current.Navigation.PopModalAsync();
            }
            else
            {
                await Shell.Current.DisplayAlert("Error!", "Error creating task group.", "OK");
            }
        }
    }

    private async Task LoadDataAsync()
    {
        TaskGroupDTO group = await _dataService.GetTaskGroupAsync(Id);
        CreateGroup = MapTaskGroupDTOToCreateTaskGroupDTO(group);
    }

    private static CreateTaskGroupDTO MapTaskGroupDTOToCreateTaskGroupDTO(TaskGroupDTO group) =>
        new()
        {
            Id = group.Id,
            Name = group.Name,
            IsFavorite = group.IsFavorite,
        };
}
