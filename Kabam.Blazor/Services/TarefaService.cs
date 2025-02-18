
using Kabam.Blazor.VM;
using Kabam.Domain;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kabam.Blazor.Services
{
    public class TarefaService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jSRuntime;
        private static readonly string TokenKey = "authToken";
        public TarefaService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jSRuntime = jsRuntime;
        }
        public async Task<bool> AddTarefaAsync(TarefaAddVM tarefa)
        {

            var token = await GetToken();
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Usuário não autenticado!");
                return false;
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // Configura a requisição com o token manualmente
            var request = new HttpRequestMessage(HttpMethod.Post, "tarefas/add")
            {
                Content = JsonContent.Create(tarefa)
            };
            //Console.Write(token);
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        public async Task<List<Tarefa>> ListarTarefas()
        {
            var token = await GetToken();
            var request = new HttpRequestMessage(HttpMethod.Get, "tarefas/getall");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(request.RequestUri);
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<List<Tarefa>>(await response.Content.ReadAsStringAsync());

            }
            return null;
        }

        async Task<string> GetToken()
        {
            var token = await _jSRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
            var jsonObject = JsonObject.Parse(token);
            return jsonObject["token"].ToString();
        }
    }
}
