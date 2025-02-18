using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Kabam.Blazor
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
       
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;
        private static readonly string TokenKey = "authToken";
        private readonly NavigationManager _navigationManager;
        public ApiAuthenticationStateProvider(IJSRuntime jsRuntime, HttpClient httpClient, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }
        public async Task MarkUserAsAuthenticated(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var authState = GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(authState);
        }
        public async Task MarkUserAsLoggedOut()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            _httpClient.DefaultRequestHeaders.Authorization = null;

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
            var identity = new ClaimsIdentity();

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims.ToList();

                identity = new ClaimsIdentity(claims, "jwt");
            }
            else
            {
                _navigationManager.NavigateTo("/login", true);
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }

                return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}
