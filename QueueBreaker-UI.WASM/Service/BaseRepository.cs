using Blazored.LocalStorage;
using QueueBreaker_UI.WASM.Contracts;
using QueueBreaker_UI.WASM.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QueueBreaker_UI.WASM.Service
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;
        public BaseRepository(HttpClient client,
            ILocalStorageService localStorage)
        {
            _client = client;
            _localStorage = localStorage;
        }
        public async Task<bool> Create(string url, T obj)
        {
            try{
                _client.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("bearer", await GetBearerToken());
                var response = await _client.PostAsJsonAsync(url, obj);

                //var content =  response.Content.ReadAsStringAsync().Result;
                //var token = JsonConvert.DeserializeObject<T>(content);
                var c = response.StatusCode;
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    return true;

                return false;
            }
            catch (Exception e)
            {
                var eee = e.Message;
                return false;

            }

        }
        //public async Task<bool> BookOrder(string url, T obj)
        //{
        //    try
        //    {
        //        _client.DefaultRequestHeaders.Authorization =
        //       new AuthenticationHeaderValue("bearer", await GetBearerToken());
        //        var response = await _client.PostAsJsonAsync(url, obj);

        //        //var content =  response.Content.ReadAsStringAsync().Result;
        //        //var token = JsonConvert.DeserializeObject<T>(content);
        //        var c = response.StatusCode;
        //        if (response.StatusCode == System.Net.HttpStatusCode.Created)
        //            return true;

        //        return false;
        //    }
        //    catch (Exception e)
        //    {
        //        var eee = e.Message;
        //        return false;

        //    }

        //}

        public async Task<bool> CreateForFile(string url, T obj)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await GetBearerToken());
            var response = await _client.PostAsJsonAsync<T>(url, obj);

            //var content =  response.Content.ReadAsStringAsync().Result;
            //var token = JsonConvert.DeserializeObject<T>(content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;

            return false;
        }


       

        public async Task<bool> Delete(string url, int id)
        {
            if (id < 1)
                return false;

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage response = await _client.DeleteAsync(url + id);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return true;

            return false;
        }

        public async Task<T> Get(string url, int id)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await GetBearerToken());
            var reponse = await _client.GetFromJsonAsync<T>(url + id);

            return reponse;
            }
            catch (Exception e)
            {
                var eee = e.Message;
                return null;

            }
        }

        public async Task<IList<T>> Get(string url)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await GetBearerToken());
                var reponse = await _client.GetFromJsonAsync<IList<T>>(url);

                return reponse;

            }
            catch (Exception e)
            {
                var eee = e.Message;
                return null;

            }
        }

        public async Task<bool> Update(string url, T obj, int id)
        {
            try
            {
                if (obj == null)
                return false;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());
            var response = await _client.PutAsJsonAsync<T>(url + id, obj);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return true;

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
            catch(Exception e)
            {
                return e.Message;
            }
        }
      
        public async Task<UserProfileModel> GetProfile(string url)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await GetBearerToken());
            var reponse = await _client.GetFromJsonAsync<UserProfileModel>(url);

            return reponse;
        }

        public async Task SetCenteenId(int CenteenId)
        {
            try
            {
                await _localStorage.SetItemAsync("centeenid", CenteenId);
            }
            catch (Exception e)
            {
                
            }

        }
         public async Task<int> GetCenteenId()
        {
            try
            {
                return await _localStorage.GetItemAsync<int>("centeenid");
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public async Task<int> GetUserId()
        {
            try
            {
                return await _localStorage.GetItemAsync<int>("userid");
            }
            catch (Exception e)
            {
                return 0;
            }
        }


    }
}
