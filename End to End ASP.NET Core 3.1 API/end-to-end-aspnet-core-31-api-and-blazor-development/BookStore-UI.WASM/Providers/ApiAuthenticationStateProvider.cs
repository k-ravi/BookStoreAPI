using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore_UI.WASM.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        readonly ILocalStorageService _localStorage;
        readonly JwtSecurityTokenHandler _securityTokenHandler;
        public ApiAuthenticationStateProvider(ILocalStorageService localStorage, JwtSecurityTokenHandler securityTokenHandler)
        {
            _localStorage = localStorage;
            _securityTokenHandler = securityTokenHandler;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var savedToken = await _localStorage.GetItemAsync<string>("authToken");
                if (string.IsNullOrWhiteSpace(savedToken))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
                var tokenContent = _securityTokenHandler.ReadJwtToken(savedToken);
                var expiry = tokenContent.ValidTo;
                if (expiry < DateTime.Now)
                {
                    await _localStorage.RemoveItemAsync("authToken");
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
                var user = new ClaimsPrincipal(new ClaimsIdentity(ParseClaims(tokenContent), "jwt"));
                return new AuthenticationState(user);
            }
            catch (Exception)
            {

                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task Loggedin()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");
            var tokenContent = _securityTokenHandler.ReadJwtToken(savedToken);
            var user = new ClaimsPrincipal(new ClaimsIdentity(ParseClaims(tokenContent), "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void LoggedOut()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }
        private IList<Claim> ParseClaims(JwtSecurityToken token)
        {
            var claims = token.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, token.Subject));
            return claims;
        }
    }
}
