using BasicTaskManagement.Core.DTO;

namespace BasicTaskManagement.Core.Services
{
    public interface IDataService
    {
        Task<IEnumerable<TaskGroupDTO>> GetTaskGroupsAsync(bool isShowComplete);
        Task<TaskGroupDTO> GetTaskGroupAsync(int id);
        Task<TaskGroupDTO> CreateTaskGroupAsync(CreateTaskGroupDTO createGroup);
    }
}
