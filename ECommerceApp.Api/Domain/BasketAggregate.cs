using System;
using System.Collections.Generic;
using System.Linq;
using ECommerceApp.Api.Domain.Commands;
using ECommerceApp.Api.Domain.Exceptions;

namespace ECommerceApp.Api.Domain
{
    public class BasketAggregate
    {
        public string Id { get; }

        public List<ProductItem> Products { get; } = new List<ProductItem>();

        public bool HasBeenUpdated { get; set; }

        public BasketAggregate(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            Id = id;
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

        public void Apply(UpdateItemCommand command)
        {
            var product = Products.SingleOrDefault(item => item.Sku == command.Sku);
            if (product == null)
            {
                throw new BasketDoesNotContainProductException();
            }

            if (product.Quantity == command.Quantity)
            {
                return;
            }

            product.Quantity = command.Quantity;
            HasBeenUpdated = true;
        }

        public void Apply(RemoveItemCommand command)
        {
            var product = Products.SingleOrDefault(item => item.Sku == command.Sku);

            if (product == null)
            {
                return;
            }
            
            Products.Remove(product);
            HasBeenUpdated = true;
        }
    }
}