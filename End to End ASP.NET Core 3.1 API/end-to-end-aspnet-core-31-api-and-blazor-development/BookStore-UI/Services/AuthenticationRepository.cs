using Blazored.LocalStorage;
using BookStore_UI.Contracts;
using BookStore_UI.Models;
using BookStore_UI.Providers;
using BookStore_UI.Static;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_UI.Services
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        readonly IHttpClientFactory _httpClient;
        readonly ILocalStorageService _localStorage;
        readonly AuthenticationStateProvider _authenticationState;
        public AuthenticationRepository(IHttpClientFactory httpClient, ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationState)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationState = authenticationState;
        }

        public async Task<bool> LogIn(LogInModel logInModel)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, EndPoints.LoginEndPoint);
            request.Content = new StringContent(JsonConvert.SerializeObject(logInModel), Encoding.UTF8, "application/json");
            var client = _httpClient.CreateClient();
            HttpResponseMessage responseMessage = await client.SendAsync(request);


            if (!responseMessage.IsSuccessStatusCode)
            {
                return false;
            }
            else
            {
                var token = JsonConvert.DeserializeObject<TokenResponse>(await responseMessage.Content.ReadAsStringAsync());
                await _localStorage.SetItemAsync("authToken", token.Token);
                await ((ApiAuthenticationStateProvider)_authenticationState).Loggedin();
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token.Token);
                return true;
            }
        }

        public async Task LogOut()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationState).LoggedOut();
        }

        public async Task<bool> Register(RegisterModel user)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, EndPoints.RegisterEndPoint);
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var client = _httpClient.CreateClient();
            HttpResponseMessage responseMessage = await client.SendAsync(request);
            return responseMessage.IsSuccessStatusCode;

        }
    }
}
