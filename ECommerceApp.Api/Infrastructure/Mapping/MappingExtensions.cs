using System.Linq;
using ECommerceApp.Api.Domain;
using ECommerceApp.Contracts.Response;

namespace ECommerceApp.Api.Infrastructure.Mapping
{
    public static class MappingExtensions
    {
        public static BasketResponse MapToDto(this BasketAggregate aggregate)
        {
            return new BasketResponse()
            {
                Id = aggregate.Id,
                Items = aggregate.Items.Select(MapToDto).ToArray()
            };
        }

        private static ProductItemResponse MapToDto(ProductItem item)
        {
            return new ProductItemResponse()
            {
                Sku = item.Sku,
                Description = item.Description,
                Quantity = item.Quantity
            };
        }
    }
}