using System.Linq;
using ECommerceApp.Api.Domain;

namespace ECommerceApp.Api.Infrastructure.Models
{
    public static class MappingExtensions
    {
        public static BasketDto MapToDto(this BasketAggregate aggregate)
        {
            return new BasketDto
            {
                Id = aggregate.Id,
                Items = aggregate.Items.Select(MapToDto).ToList()
            };
        }

        private static ProductDto MapToDto(ProductItem item)
        {
            return new ProductDto
            {
                Sku = item.Sku,
                Description = item.Description,
                Quantity = item.Quantity
            };
        }
    }
}