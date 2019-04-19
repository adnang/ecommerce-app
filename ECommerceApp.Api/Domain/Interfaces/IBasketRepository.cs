using System.Threading;
using System.Threading.Tasks;

namespace ECommerceApp.Api.Domain.Interfaces
{
    public interface IBasketRepository
    {
        Task<BasketAggregate> Load(string id, CancellationToken cancellationToken);
        
        Task Save(BasketAggregate basketAggregate, CancellationToken cancellationToken);
    }
}