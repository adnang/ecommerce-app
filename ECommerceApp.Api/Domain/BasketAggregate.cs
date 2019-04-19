using System;
using System.Collections.Generic;
using System.Linq;
using ECommerceApp.Api.Domain.Commands;

namespace ECommerceApp.Api.Domain
{
    public class BasketAggregate
    {
        public string Id { get; }

        public string CustomerId { get; }

        public List<ProductItem> Products { get; } = new List<ProductItem>();

        public bool HasBeenUpdated { get; set; }

        public BasketAggregate(string id, string customerId)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException();
            }

            Id = id;
            CustomerId = customerId;
        }

        public void Apply(AddItemCommand command)
        {
            if (Products.Any(item => item.Sku == command.Sku))
            {
                return;
            }

            Products.Add(new ProductItem
            {
                Sku = command.Sku,
                Description = command.Description
            });

            HasBeenUpdated = true;
        }
    }

    public class ProductItem
    {
        public string Sku { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; } = 1;
    }
}