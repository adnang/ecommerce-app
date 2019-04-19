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

        public async Task<BasketAggregate> Load(string id, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Cache.GetOrCreate(
                id,
                entry => new BasketAggregate
                (
                    id
                )));
        }

        public async Task Save(BasketAggregate basketAggregate, CancellationToken cancellationToken)
        {
            if (basketAggregate.HasBeenUpdated)
            {
                Cache.Set(basketAggregate.Id, basketAggregate);
            }

            await Task.CompletedTask;
        }
    }
}