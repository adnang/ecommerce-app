using System.Collections.Generic;
using ECommerceApp.Api.Domain;

namespace ECommerceApp.Api.Infrastructure.Models
{
    public class BasketDto
    {
        public string Id { get; set; }
        public List<ProductDto> Items { get; set; }
    }
}