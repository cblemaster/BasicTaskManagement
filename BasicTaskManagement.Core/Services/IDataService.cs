using BasicTaskManagement.Core.DTO;

namespace BasicTaskManagement.Core.Services;

public interface IDataService
{
    Task<IEnumerable<TaskGroupSummaryDTO?>> GetTaskGroupsAsync();
    Task<TaskGroupDTO> GetTaskGroupAsync(int id);
    Task<IEnumerable<TaskGroupDTO?>> GetTaskGroupsForManagementAsync();
    Task<TaskGroupDTO> CreateTaskGroupAsync(CreateTaskGroupDTO createGroup);
    Task DeleteTaskGroupAsync(int id);
    Task UpdateTaskGroupAsync(int id, CreateTaskGroupDTO dto);
    Task<TaskItemDTO> GetTaskItemAsync(int id);
    Task<IEnumerable<TaskItemDTO?>> GetImportantTaskItemsAsync(bool isShowComplete);
    Task<IEnumerable<TaskItemDTO?>> GetTaskItemsDueTodayAsync(bool isShowComplete);
    Task<IEnumerable<TaskItemDTO?>> GetCompletedTaskItemsAsync();
    Task<TaskItemDTO> CreateTaskItemAsync(CreateUpdateTaskItemDTO createItem);
    Task DeleteTaskItemAsync(int id);
    Task UpdateTaskItemAsync(int id, CreateUpdateTaskItemDTO dto);

}
