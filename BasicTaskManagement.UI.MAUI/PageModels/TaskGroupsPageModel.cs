using BasicTaskManagement.Core.DataModels;
using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using BasicTaskManagement.UI.MAUI.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class TaskGroupsPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private ReadOnlyCollection<TaskGroupData> _taskGroups = default;

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
    private async Task GroupsSelectionChanged() => await Shell.Current.Navigation.PushModalAsync(new TaskItemPage(SelectedTaskItem.Id));

    [RelayCommand]
    private static async Task CreateGroupClicked() => await Shell.Current.Navigation.PushModalAsync(new CreateTaskGroupPage());

    [RelayCommand]
    private void ShowCompletedFilterChanged()
    {
        IsShowComplete = !IsShowComplete;
        LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        IEnumerable<TaskGroupDTO> groups = await _dataService.GetTaskGroupsAsync(IsShowComplete);
        TaskGroups = MapTaskGroupDTOCollectionToTaskGroupDataCollection(groups).ToList().AsReadOnly();
    }

    public static IEnumerable<TaskGroupData> MapTaskGroupDTOCollectionToTaskGroupDataCollection(IEnumerable<TaskGroupDTO> dtos)
    {
        List<TaskGroupData> collectionToReturn = [];

        foreach (TaskGroupDTO dto in dtos)
        {
            TaskGroupData data = MapTaskGroupDTOToTaskGroupData(dto);
            collectionToReturn.Add(data);
        }

        return collectionToReturn.AsEnumerable();

        static TaskGroupData MapTaskGroupDTOToTaskGroupData(TaskGroupDTO dto) => new(dto.Id, dto.Name, dto.IsFavorite, dto.TaskItems.ToList());
    }
}
