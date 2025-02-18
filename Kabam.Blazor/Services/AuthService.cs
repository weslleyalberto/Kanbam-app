﻿using Blazored.LocalStorage;
using Kabam.Blazor.Models;
using Kabam.Domain;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace Kabam.Blazor.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }
        public async Task<RegisterResult> Register(RegisterModel registerModel)
        {
            var messageResult = await _httpClient.PostAsJsonAsync("register/create", registerModel);
            var result = await messageResult.Content.ReadFromJsonAsync<RegisterResult>();
            return result;
        }
        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var loginAsJson = JsonSerializer.Serialize(loginModel);
            var response = await _httpClient.PostAsync("login/login",
                new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
            var loginResult = JsonSerializer.Deserialize<LoginResult>(await
                response.Content.ReadAsStringAsync(), new JsonSerializerOptions
                { PropertyNameCaseInsensitive = true });
            if (!response.IsSuccessStatusCode)
            {
                return loginResult;
            }
            await _localStorage.SetItemAsync("authToken", loginResult.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider)
                .MarkUserAsAuthenticated(loginModel.Email);
            _httpClient.DefaultRequestHeaders.Authorization = new
                AuthenticationHeaderValue("bearer", loginResult.Token);
            return loginResult;
        }
        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
