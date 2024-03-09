using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class ManageTaskGroupsPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private IReadOnlyCollection<TaskGroupDTO> _taskGroups;

    [ObservableProperty]
    private TaskGroupDTO selectedTaskGroup;

    [RelayCommand]
    private async Task PageAppearing() => await LoadDataAsync();

    private async Task LoadDataAsync() => TaskGroups = (await _dataService.GetTaskGroupsForManagementAsync()).ToList().AsReadOnly();
}
