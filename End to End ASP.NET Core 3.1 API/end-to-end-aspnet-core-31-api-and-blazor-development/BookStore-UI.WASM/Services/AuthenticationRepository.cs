using Blazored.LocalStorage;
using BookStore_UI.WASM.Contracts;
using BookStore_UI.WASM.Models;
using BookStore_UI.WASM.Providers;
using BookStore_UI.WASM.Static;
using Microsoft.AspNetCore.Components.Authorization;
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
    public class AuthenticationRepository : IAuthenticationRepository
    {
        readonly HttpClient _httpClient;
        readonly ILocalStorageService _localStorage;
        readonly AuthenticationStateProvider _authenticationState;
        public AuthenticationRepository(HttpClient httpClient, ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationState)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationState = authenticationState;
        }

        public async Task<bool> LogIn(LogInModel logInModel)
        {
            HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync<LogInModel>(EndPoints.LoginEndPoint,logInModel);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return false;
            }
            else
            {
                var token = JsonConvert.DeserializeObject<TokenResponse>(await responseMessage.Content.ReadAsStringAsync());
                await _localStorage.SetItemAsync("authToken", token.Token);
                await ((ApiAuthenticationStateProvider)_authenticationState).Loggedin();
                _httpClient.DefaultRequestHeaders.Authorization =
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
            HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync<RegisterModel>(EndPoints.RegisterEndPoint,user);
            return responseMessage.IsSuccessStatusCode;

        }
    }
}
