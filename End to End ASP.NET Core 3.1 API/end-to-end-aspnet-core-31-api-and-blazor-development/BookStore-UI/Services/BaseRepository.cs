﻿using Blazored.LocalStorage;
using BookStore_UI.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_UI.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILocalStorageService _localStorage;
        public BaseRepository(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _localStorage = localStorage;
        }
        public async Task<bool> Create(string url, T obj)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (obj == null)
            {
                return false;
            }
            request.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage responseMessage = await client.SendAsync(request);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Created)
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
            var request = new HttpRequestMessage(HttpMethod.Delete, url + "/" + id);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage responseMessage = await client.SendAsync(request);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }

        public async Task<T> Get(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + "/" + id);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage responseMessage = await client.SendAsync(request);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            return null;
        }

        public async Task<IList<T>> Get(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage responseMessage = await client.SendAsync(request);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IList<T>>(content);
            }
            return null;
            throw new NotImplementedException();
        }

        public async Task<bool> Update(string url, T obj, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url + "/" + id);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearerToken());
            request.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.SendAsync(request);
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
