using BasicTaskManagement.Core.DTO;
using System.Net.Http.Json;
using System.Text.Json;

namespace BasicTaskManagement.Core.Services
{
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

            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"/taskgroup/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception) { throw; }
        }
    }
}
