using System;
using ECommerceApp.Api.Domain;
using ECommerceApp.Api.Domain.Commands;
using FluentAssertions;
using Xunit;

namespace ECommerceApp.UnitTests.Domain
{
    public class BasketAggregateShould
    {
        private const string CustomerId = "CustomerId";
        private const string AggregateId = "AggregateId";

        [Fact]
        public void BeInitialized_GivenValidConstructor()
        {
            var basketAggregate = new BasketAggregate(AggregateId, CustomerId);
            basketAggregate.Id.Should().Be(AggregateId);
            basketAggregate.CustomerId.Should().Be(CustomerId);
        }
        
        [Theory]
        [InlineData(null, CustomerId)]
        [InlineData(AggregateId, null)]
        [InlineData(null, null)]
        public void ThrowException_GivenInvalidConstructor(string id, string customerId)
        {
            Assert.Throws<ArgumentNullException>(() => new BasketAggregate(id, customerId));
        }
        
        [Fact]
        public void BeUpdated_WhenAddingNewProduct()
        {
            var basketAggregate = new BasketAggregate(AggregateId, CustomerId);

            var addItemCommand = new AddItemCommand
            {
                Sku = "12345",
                Description = "Product Description"
            };
            
            basketAggregate.Apply(addItemCommand);

            basketAggregate.HasBeenUpdated.Should().BeTrue();
            basketAggregate.Products.Should()
                .ContainSingle(item => item.Quantity == 1)
                .And.AllBeEquivalentTo(addItemCommand);
        }

        [Fact]
        public void BeIdempotent_WhenAddingExistingProduct()
        {
            var basketAggregate = new BasketAggregate(AggregateId, CustomerId);

            var addItemCommand = new AddItemCommand
            {
                Sku = "12345",
                Description = "Product Description"
            };
            
            basketAggregate.Apply(addItemCommand);
            basketAggregate.HasBeenUpdated = false;
            
            basketAggregate.Apply(addItemCommand);

            basketAggregate.Products.Should()
                .ContainSingle(item => item.Quantity == 1);
            basketAggregate.HasBeenUpdated.Should().BeFalse();
        }
    }
}