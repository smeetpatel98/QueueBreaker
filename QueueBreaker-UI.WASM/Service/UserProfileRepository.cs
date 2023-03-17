using Blazored.LocalStorage;
using QueueBreaker_UI.WASM.Contracts;
using QueueBreaker_UI.WASM.Models;
using QueueBreaker_UI.WASM.Static;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QueueBreaker_UI.WASM.Service
{
    public class UserProfileRepository : BaseRepository<UserProfileModel>, IUserProfileRepository
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;
        public UserProfileRepository(HttpClient client,
            ILocalStorageService localStorage) : base(client, localStorage)
        {
            _client = client;
            _localStorage = localStorage;
        }

        public async Task<bool> UpdateUserPassword(UpdatePasswordModel user)
        {
            var response = await _client.PostAsJsonAsync(Endpoints.UserProfileEndpoint
                , user);
            return response.IsSuccessStatusCode;

        }

    }
}
