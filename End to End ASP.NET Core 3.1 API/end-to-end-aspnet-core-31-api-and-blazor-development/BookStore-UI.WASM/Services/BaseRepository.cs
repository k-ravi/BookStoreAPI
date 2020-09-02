using Blazored.LocalStorage;
using BookStore_UI.WASM.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_UI.WASM.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        readonly HttpClient _httpClientFactory;
        readonly ILocalStorageService _localStorage;
        public BaseRepository(HttpClient  httpClientFactory, ILocalStorageService localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _localStorage = localStorage;
        }
        public async Task<bool> Create(string url, T obj)
        {
            if (obj == null)
            {
                return false;
            }
            _httpClientFactory.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage responseMessage = await _httpClientFactory.PostAsJsonAsync(url,obj);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(string url, int id)
        {
            if (id < 1)
            {
                return false;
            }
            _httpClientFactory.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage responseMessage = await _httpClientFactory.DeleteAsync(url + "/" + id);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }

        public async Task<T> Get(string url, int id)
        {
            _httpClientFactory.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearerToken());
            var responseMessage = await _httpClientFactory.GetFromJsonAsync<T>(url + "/" + id);
            return responseMessage;
        }

        public async Task<IList<T>> Get(string url)
        {
            _httpClientFactory.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearerToken());
            var responseMessage = await _httpClientFactory.GetFromJsonAsync<IList<T>>(url );
            return responseMessage;
        }

        public async Task<bool> Update(string url, T obj, int id)
        {
            _httpClientFactory.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage responseMessage = await _httpClientFactory.PutAsJsonAsync<T>(url + "/" + id,obj);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;

        }
        async Task<string> GetBearerToken()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }
    }
}
