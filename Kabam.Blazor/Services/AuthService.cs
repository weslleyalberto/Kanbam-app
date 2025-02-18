using Kabam.Blazor.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace Kabam.Blazor.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiAuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = (ApiAuthenticationStateProvider)authStateProvider;
        }

        public async Task<bool> Login(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/login/login", request);

            if (!response.IsSuccessStatusCode) return false;

            var token = await response.Content.ReadAsStringAsync();
            await _authStateProvider.MarkUserAsAuthenticated(token);
            return true;
        }
        public async Task Logout()
        {
            await _authStateProvider.MarkUserAsLoggedOut();
        }
    }
}
