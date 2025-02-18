

using Kabam.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kabam.Blazor.Services
{
    public class TarefaService
    {
        private readonly NavigationManager _navigationManager;
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jSRuntime;
        private static readonly string TokenKey = "authToken";
        public TarefaService(HttpClient httpClient, IJSRuntime jsRuntime,
            NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _jSRuntime = jsRuntime;
            _navigationManager = navigationManager;
        }
        public async Task<bool> AddTarefaAsync(Tarefa tarefa)
        {

            var tarefaContent = JsonSerializer.Serialize(tarefa);
            var response = await _httpClient.PostAsync("tarefas/add",
                new StringContent(tarefaContent, System.Text.Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //// Configura a requisição com o token manualmente
            //var request = new HttpRequestMessage(HttpMethod.Post, "tarefas/add")
            //{
            //    Content = JsonContent.Create(tarefa)
            //};
            ////Console.Write(token);
            ////request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //var response = await _httpClient.SendAsync(request);
            //return response.IsSuccessStatusCode;
        }
     
        public async Task<List<Tarefa>> GeTarefasAsync()
        {
            var response = await _httpClient.GetAsync("tarefas/getall");
            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _navigationManager.NavigateTo("notauthorized");
            }
            var result = await _httpClient.GetFromJsonAsync<List<Tarefa>>("tarefas/getall");
            return result;

        }

       
    }
}
