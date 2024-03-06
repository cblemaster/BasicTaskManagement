using BasicTaskManagement.Core.DataModels;
using BasicTaskManagement.Core.DTO;
using System.Collections.ObjectModel;

namespace BasicTaskManagement.Core.Services
{
    public interface IDataService
    {
        Task<ReadOnlyCollection<TaskGroupData>> GetTaskGroupsAsync(bool isShowComplete);
        Task<TaskGroupDTO> GetTaskGroupAsync(int id);
        Task<TaskGroupDTO> CreateTaskGroupAsync(CreateTaskGroupDTO createGroup);
    }
}
