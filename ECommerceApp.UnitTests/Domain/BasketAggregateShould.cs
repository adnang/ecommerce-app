using System;
using ECommerceApp.Api.Domain;
using ECommerceApp.Api.Domain.Commands;
using ECommerceApp.Api.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace ECommerceApp.UnitTests.Domain
{
    public class BasketAggregateShould
    {
        private readonly AddItemCommand _addItemCommand;
        private readonly UpdateItemCommand _updateItemCommand;
        private const string AggregateId = "AggregateId";

        public BasketAggregateShould()
        {
            _addItemCommand = new AddItemCommand
            {
                Sku = "12345",
                Description = "Product Description"
            };

            _updateItemCommand = new UpdateItemCommand
            {
                Sku = _addItemCommand.Sku,
                Quantity = 2
            };
        }

        [Fact]
        public void BeInitialized_GivenValidConstructor()
        {
            var basketAggregate = new BasketAggregate(AggregateId);
            basketAggregate.Id.Should().Be(AggregateId);
            basketAggregate.Items.Should().BeEmpty();
        }

        [Fact]
        public void ThrowException_GivenInvalidConstructor()
        {
            Action action = () => new BasketAggregate(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void BeUpdated_WhenAddingNewItem()
        {
            var basketAggregate = new BasketAggregate(AggregateId);

            basketAggregate.Apply(_addItemCommand);

            basketAggregate.HasBeenUpdated.Should().BeTrue();
            basketAggregate.Items.Should()
                .ContainSingle(item => item.Quantity == 1)
                .And.AllBeEquivalentTo(_addItemCommand);
        }

        [Fact]
        public void BeIdempotent_WhenAddingExistingItem()
        {
            var basketAggregate = new BasketAggregate(AggregateId);

            basketAggregate.Apply(_addItemCommand);
            basketAggregate.HasBeenUpdated = false;

            basketAggregate.Apply(_addItemCommand);

            basketAggregate.Items.Should()
                .ContainSingle(item => item.Quantity == 1);
            basketAggregate.HasBeenUpdated.Should().BeFalse();
        }

        [Fact]
        public void BeUpdated_WhenUpdatingExistingItem()
        {
            var basketAggregate = new BasketAggregate(AggregateId);
            basketAggregate.Apply(_addItemCommand);
            basketAggregate.HasBeenUpdated = false;

            basketAggregate.Apply(_updateItemCommand);

            basketAggregate.HasBeenUpdated.Should().BeTrue();
            basketAggregate.Items.Should().ContainSingle(
                item => item.Sku == _addItemCommand.Sku && item.Quantity == _updateItemCommand.Quantity);
        }

        [Fact]
        public void ThrowException_WhenUpdatingNonExistingItem()
        {
            var basketAggregate = new BasketAggregate(AggregateId);
            basketAggregate.Apply(_addItemCommand);
            basketAggregate.HasBeenUpdated = false;

            Action action = () => basketAggregate.Apply(new UpdateItemCommand
            {
                Sku = "54321",
                Quantity = 2
            });

            action.Should().Throw<BasketDoesNotContainProductException>();
        }

        [Fact]
        public void BeIdempotent_WhenUpdatingToTheSameQuantity()
        {
            var basketAggregate = new BasketAggregate(AggregateId);
            basketAggregate.Apply(_addItemCommand);
            basketAggregate.Apply(_updateItemCommand);
            basketAggregate.HasBeenUpdated = false;

            basketAggregate.Apply(_updateItemCommand);

            basketAggregate.HasBeenUpdated.Should().BeFalse();
            basketAggregate.Items.Should().ContainSingle(
                item => item.Sku == _addItemCommand.Sku && item.Quantity == _updateItemCommand.Quantity);
        }

        [Fact]
        public void BeUpdated_WhenRemovingAnExistingItem()
        {
            var basketAggregate = new BasketAggregate(AggregateId);
            basketAggregate.Apply(_addItemCommand);
            basketAggregate.HasBeenUpdated = false;

            basketAggregate.Apply(new RemoveItemCommand
            {
                Sku = _addItemCommand.Sku
            });

            basketAggregate.HasBeenUpdated.Should().BeTrue();
            basketAggregate.Items.Should().NotContain(item => item.Sku == _addItemCommand.Sku);
        }
        
        [Fact]
        public void BeIdempotent_WhenRemovingNonExistingProduct()
        {
            var basketAggregate = new BasketAggregate(AggregateId);
            basketAggregate.Apply(new RemoveItemCommand
            {
                Sku = _addItemCommand.Sku
            });

            basketAggregate.HasBeenUpdated.Should().BeFalse();
            basketAggregate.Items.Should().NotContain(item => item.Sku == _addItemCommand.Sku);
        }

        [Fact]
        public void BeUpdated_WhenClearingBasketWithItems()
        {
            var basketAggregate = new BasketAggregate(AggregateId);
            basketAggregate.Apply(_addItemCommand);
            basketAggregate.HasBeenUpdated = false;
            
            basketAggregate.Apply(new ClearItemsCommand());

            basketAggregate.HasBeenUpdated.Should().BeTrue();
            basketAggregate.Items.Should().BeEmpty();
        }
        
        [Fact]
        public void BeIdempotent_WhenClearingEmptyBasket()
        {
            var basketAggregate = new BasketAggregate(AggregateId);
            
            basketAggregate.Apply(new ClearItemsCommand());

            basketAggregate.HasBeenUpdated.Should().BeFalse();
            basketAggregate.Items.Should().BeEmpty();
        }
    }
}