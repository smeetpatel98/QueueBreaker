using Blazored.LocalStorage;
using QueueBreaker_UI.WASM.Contracts;
using QueueBreaker_UI.WASM.Models;
using QueueBreaker_UI.WASM.Providers;
using QueueBreaker_UI.WASM.Static;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QueueBreaker_UI.WASM.Service
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationRepository(HttpClient client,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _client = client;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> Login(LoginModel user)
        {
            var response = await _client.PostAsJsonAsync(Endpoints.LoginEndpoint, user);
            Console.WriteLine(response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TokenResponse>(content);

            //Store Token
            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("userid", result.UserId);
            await _localStorage.SetItemAsync("centeenid", result.CenteenId);

            //Change auth state of app
            await ((ApiAuthenticationStateProvider)_authenticationStateProvider)
                .LoggedIn();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", result.Token);

            return true;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("userid");
            await _localStorage.RemoveItemAsync("centeenid");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider)
                .LoggedOut();
        }

        public async Task<bool> Register(RegistrationModel user)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());
                var response = await _client.PostAsJsonAsync(Endpoints.RegisterEndpoint, user);

                //var content =  response.Content.ReadAsStringAsync().Result;
                //var token = JsonConvert.DeserializeObject<T>(content);
                var c = response.StatusCode;
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    return true; //return response.IsSuccessStatusCode;

                return false;
            }
            catch (Exception e)
            {
                var eee = e.Message;
                return false;

            }
           

        }

        private async Task<string> GetBearerToken()
        {
            try
            {
                return await _localStorage.GetItemAsync<string>("authToken");
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
       
    }
}
