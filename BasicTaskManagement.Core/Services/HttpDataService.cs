using BasicTaskManagement.Core.DTO;
using System.Net.Http.Json;
using System.Text.Json;

namespace BasicTaskManagement.Core.Services;

public class HttpDataService : IDataService
{
    private readonly HttpClient _client;
    private const string BASE_URI = "https://localhost:7114";

    public HttpDataService() => _client = new HttpClient
    {
        BaseAddress = new Uri(BASE_URI)
    };

    public async Task<IEnumerable<TaskGroupDTO>> GetTaskGroupsAsync(bool isShowComplete)
    {
        List<TaskGroupDTO> groups = [];

        string route = isShowComplete ? "/taskgroup/showcomplete" : "/taskgroup";
        try
        {
            HttpResponseMessage response = await _client.GetAsync(route);
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                groups = response.Content.ReadFromJsonAsAsyncEnumerable<TaskGroupDTO>().ToBlockingEnumerable().ToList();
            }
            return groups is not null ? groups.AsReadOnly() : Enumerable.Empty<TaskGroupDTO>().ToList().AsReadOnly();
        }
        catch (Exception) { throw; }
    }

    public async Task<IEnumerable<TaskGroupDTO>> GetTaskGroupsForManagementAsync()
    {
        List<TaskGroupDTO> groups = [];

        try
        {
            HttpResponseMessage response = await _client.GetAsync("/taskgroup/manage");
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                groups = response.Content.ReadFromJsonAsAsyncEnumerable<TaskGroupDTO>().ToBlockingEnumerable().ToList();
            }
            return groups is not null ? groups.AsReadOnly() : Enumerable.Empty<TaskGroupDTO>().ToList().AsReadOnly();
        }
        catch (Exception) { throw; }
    }

    public async Task<TaskGroupDTO> GetTaskGroupAsync(int id)
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync($"/taskgroup/{id}");
            return response.IsSuccessStatusCode && response.Content is not null
                ? await response.Content.ReadFromJsonAsync<TaskGroupDTO>() ?? TaskGroupDTO.NotFound
                : TaskGroupDTO.NotFound;
        }
        catch (Exception) { throw; }
    }

    public async Task<TaskGroupDTO> CreateTaskGroupAsync(CreateTaskGroupDTO createGroup)
    {
        IEnumerable<string> list = (await GetTaskGroupsAsync(true)).Select(tg => tg.Name);
        if (list.Contains(createGroup.Name)) { return TaskGroupDTO.NotFound; }  //TODO: Should really return an object representing the error...
        
        StringContent content = new(JsonSerializer.Serialize(createGroup));
        content.Headers.ContentType = new("application/json");

        try
        {
            HttpResponseMessage response = await _client.PostAsync("/taskgroup", content);
            response.EnsureSuccessStatusCode();
            return await GetTaskGroupAsync(await response.Content.ReadFromJsonAsync<TaskGroupDTO>() is TaskGroupDTO newGroup ? newGroup.Id : 0);
        }
        catch (Exception) { throw; }
    }

    public async Task DeleteTaskGroupAsync(int id)
    {
        if (id < 1) { return; }

        var groups = await GetTaskGroupsAsync(true);
        var groupNames = groups.Select(g => g.Name).ToList();
        var group = await GetTaskGroupAsync(id);
                
        if (groupNames.Contains(group.Name)) { return; }

        try
        {
            HttpResponseMessage response = await _client.DeleteAsync($"/taskgroup/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception) { throw; }
    }

    public async Task<IEnumerable<TaskItemDTO>> GetImportantTaskItemsAsync(bool isShowComplete)
    {
        List<TaskItemDTO> items = [];

        string route = isShowComplete ? "/taskitem/important/showcomplete" : "/taskitem/important";
        try
        {
            HttpResponseMessage response = await _client.GetAsync(route);
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                items = response.Content.ReadFromJsonAsAsyncEnumerable<TaskItemDTO>().ToBlockingEnumerable().ToList();
            }
            return items is not null ? items.AsReadOnly() : Enumerable.Empty<TaskItemDTO>().ToList().AsReadOnly();
        }
        catch (Exception) { throw; }
    }

    public async Task<IEnumerable<TaskItemDTO>> GetTaskItemsDueTodayAsync(bool isShowComplete)
    {
        List<TaskItemDTO> items = [];

        string route = isShowComplete ? "/taskitem/duetoday/showcomplete" : "/taskitem/duetoday";
        try
        {
            HttpResponseMessage response = await _client.GetAsync(route);
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                items = response.Content.ReadFromJsonAsAsyncEnumerable<TaskItemDTO>().ToBlockingEnumerable().ToList();
            }
            return items is not null ? items.AsReadOnly() : Enumerable.Empty<TaskItemDTO>().ToList().AsReadOnly();
        }
        catch (Exception) { throw; }
    }

    public async Task<IEnumerable<TaskItemDTO>> GetCompletedTaskItemsAsync()
    {
        List<TaskItemDTO> items = [];

        try
        {
            HttpResponseMessage response = await _client.GetAsync("/taskitem/completed");
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                items = response.Content.ReadFromJsonAsAsyncEnumerable<TaskItemDTO>().ToBlockingEnumerable().ToList();
            }
            return items is not null ? items.AsReadOnly() : Enumerable.Empty<TaskItemDTO>().ToList().AsReadOnly();
        }
        catch (Exception) { throw; }
    }

    public async Task<TaskItemDTO> GetTaskItemAsync(int id)
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync($"/taskitem/{id}");
            return response.IsSuccessStatusCode && response.Content is not null
                ? await response.Content.ReadFromJsonAsync<TaskItemDTO>() ?? TaskItemDTO.NotFound
                : TaskItemDTO.NotFound;
        }
        catch (Exception) { throw; }
    }

    public async Task DeleteTaskItemAsync(int id)
    {
        try
        {
            HttpResponseMessage response = await _client.DeleteAsync($"/taskitem/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception) { throw; }
    }

    public async Task UpdateTaskItemAsync(int id, CreateUpdateTaskItemDTO dto)
    {
        if ((await GetTaskItemAsync(id)).IsComplete) { return; }
        
        if (dto is null || id < 1 || id != dto.Id) { return; }

        StringContent content = new(JsonSerializer.Serialize(dto));
        content.Headers.ContentType = new("application/json");

        try
        {
            HttpResponseMessage response = await _client.PutAsync($"/taskitem/{id}", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception) { throw; }
    }

    public async Task<TaskItemDTO> CreateTaskItemAsync(CreateUpdateTaskItemDTO createItem)
    {
        StringContent content = new(JsonSerializer.Serialize(createItem));
        content.Headers.ContentType = new("application/json");

        try
        {
            HttpResponseMessage response = await _client.PostAsync("/taskitem", content);
            response.EnsureSuccessStatusCode();
            return await GetTaskItemAsync(await response.Content.ReadFromJsonAsync<TaskItemDTO>() is TaskItemDTO newItem ? newItem.Id : 0);
        }
        catch (Exception) { throw; }
    }

    public async Task UpdateTaskGroupAsync(int id, CreateTaskGroupDTO dto)
    {
        if (dto is null || id < 1 || id != dto.Id) { return; }

        StringContent content = new(JsonSerializer.Serialize(dto));
        content.Headers.ContentType = new("application/json");

        try
        {
            HttpResponseMessage response = await _client.PutAsync($"/taskgroup/{id}", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception) { throw; }
    }
}
