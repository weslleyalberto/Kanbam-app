using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace Kabam.Blazor
{
    public class AuthHttpMessageHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;
        private static readonly string TokenKey = "authToken";

        public AuthHttpMessageHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);

            if (!string.IsNullOrEmpty(token))
            {
                var jsonObject = JsonObject.Parse(token);
                token = jsonObject["token"].ToString();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
