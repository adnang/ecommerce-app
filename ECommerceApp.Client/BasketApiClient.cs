using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ECommerceApp.Client.Configuration;
using ECommerceApp.Client.Requests;
using ECommerceApp.Contracts.Response;
using Newtonsoft.Json;

namespace ECommerceApp.Client
{
    public class BasketApiClient : IBasketApiClient
    {
        private readonly HttpClient _httpClient;

        public BasketApiClient(BasketApiConfiguration configuration)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = configuration.BaseAddress,
                Timeout = configuration.ConnectionTimeout,
                DefaultRequestHeaders = {{"ContentType", "application/json"}}
            };
        }

        public async Task<BasketResponse> GetBasket(string id, CancellationToken cancellationToken)
        {
            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Get, $"/api/v1/basket/{id}");

            var responseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);

            responseMessage.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<BasketResponse>(await responseMessage.Content.ReadAsStringAsync());
        }
        
        
        public async Task AddProduct(string id, AddProductRequest request, CancellationToken cancellationToken)
        {
            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Put, $"/api/v1/basket/{id}/items/add")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(request))
                };

            var responseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
        }
        
        public async Task UpdateProduct(string id, UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Put, $"/api/v1/basket/{id}/items/update")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(request))
                };

            var responseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
        }
        
        public async Task RemoveProduct(string id, RemoveProductRequest request, CancellationToken cancellationToken)
        {
            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Put, $"/api/v1/basket/{id}/items/remove")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(request))
                };

            var responseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
        }
        
        public async Task ClearItems(string id, CancellationToken cancellationToken)
        {
            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/basket/{id}");

            var responseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
        }

    }
}