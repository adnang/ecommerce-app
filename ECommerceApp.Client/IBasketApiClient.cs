using System.Threading;
using System.Threading.Tasks;
using ECommerceApp.Client.Requests;
using ECommerceApp.Contracts.Response;

namespace ECommerceApp.Client
{
    public interface IBasketApiClient
    {
        Task<BasketResponse> GetBasket(string id, CancellationToken cancellationToken);
        
        Task AddProduct(string id, AddProductRequest request, CancellationToken cancellationToken);
        
        Task UpdateProduct(string id, UpdateProductRequest request, CancellationToken cancellationToken);
        
        Task RemoveProduct(string id, RemoveProductRequest request, CancellationToken cancellationToken);
        
        Task ClearItems(string id, CancellationToken cancellationToken);
    }
}