using System;
using System.Threading;
using System.Threading.Tasks;
using ECommerceApp.Api.Domain;
using ECommerceApp.Api.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ECommerceApp.Api.Infrastructure
{
    public class InMemoryBasketRepository : IBasketRepository
    {
        private static readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());

        public async Task<BasketAggregate> LoadByCustomer(string customerId, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Cache.GetOrCreate(
                customerId,
                entry => new BasketAggregate
                (
                    Guid.NewGuid().ToString(),
                    customerId
                )));
        }

        public async Task Save(BasketAggregate basketAggregate, CancellationToken cancellationToken)
        {
            if (basketAggregate.HasBeenUpdated)
            {
                Cache.Set(basketAggregate.CustomerId, basketAggregate);
            }

            await Task.CompletedTask;
        }
    }
}