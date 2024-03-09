using BasicTaskManagement.Core.DataModels;
using BasicTaskManagement.Core.DTO;
using BasicTaskManagement.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BasicTaskManagement.UI.MAUI.PageModels;

public partial class ManageTaskGroupsPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private IReadOnlyCollection<TaskGroupSummaryDTO> _taskGroups;

    [ObservableProperty]
    private TaskGroupDTO selectedTaskGroup;

    [RelayCommand]
    private async Task PageAppearing() => await LoadDataAsync();

    private async Task LoadDataAsync()
    {
        IEnumerable<TaskGroupDTO> groups = (await _dataService.GetTaskGroupsForManagementAsync());
        TaskGroups = MapTaskGroupDTOCollectionToTaskGroupSummaryDTOCollection(groups).ToList().AsReadOnly();
    }

    private static IEnumerable<TaskGroupSummaryDTO> MapTaskGroupDTOCollectionToTaskGroupSummaryDTOCollection(IEnumerable<TaskGroupDTO> dtos)
    {
        List<TaskGroupSummaryDTO> collectionToReturn = [];

        foreach (TaskGroupDTO dto in dtos)
        {
            TaskGroupSummaryDTO summary = MapTaskGroupDTOToTaskGroupSummaryDTO(dto);
            collectionToReturn.Add(summary);
        }

        return collectionToReturn.AsEnumerable();

        static TaskGroupSummaryDTO MapTaskGroupDTOToTaskGroupSummaryDTO(TaskGroupDTO taskGroupDTO)
        {
            return new()
            {
                Id = taskGroupDTO.Id,
                Name = taskGroupDTO.Name,
                IsFavorite = taskGroupDTO.IsFavorite,
                CountOfTaskItems = taskGroupDTO.TaskItems.Count(),
            };
        }
    }
}
