﻿using BasicTaskManagement.Core.DTO;

namespace BasicTaskManagement.Core.Services;

public interface IDataService
{
    Task<IEnumerable<TaskGroupDTO>> GetTaskGroupsAsync(bool isShowComplete);
    Task<TaskGroupDTO> GetTaskGroupAsync(int id);
    Task<TaskGroupDTO> CreateTaskGroupAsync(CreateTaskGroupDTO createGroup);
    Task DeleteTaskGroupAsync(int id);
    Task<IEnumerable<TaskItemDTO>> GetImportantTaskItemsAsync(bool isShowComplete);
    Task<IEnumerable<TaskItemDTO>> GetTaskItemsDueTodayAsync(bool isShowComplete);
    Task<IEnumerable<TaskItemDTO>> GetCompletedTaskItemsAsync();
    Task<TaskItemDTO> GetTaskItemAsync(int id);
    Task DeleteTaskItemAsync(int id);
    Task UpdateTaskItemAsync(int id, CreateUpdateTaskItemDTO dto);
    Task<TaskItemDTO> CreateTaskItemAsync(CreateUpdateTaskItemDTO createItem);
    Task<IEnumerable<TaskGroupDTO>> GetTaskGroupsForManagementAsync();
}
