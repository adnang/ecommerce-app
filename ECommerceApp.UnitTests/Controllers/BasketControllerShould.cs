using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ECommerceApp.Api.Controllers;
using ECommerceApp.Api.Domain;
using ECommerceApp.Api.Domain.Commands;
using ECommerceApp.Api.Domain.Interfaces;
using ECommerceApp.Contracts.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ECommerceApp.UnitTests.Controllers
{
    public class BasketControllerShould
    {
        private const string Id = "id";
        private readonly Mock<IBasketRepository> _repositoryMock;
        private readonly BasketAggregate _basketAggregate;
        private readonly BasketControllerV1 _controller;

        public BasketControllerShould()
        {
            _repositoryMock = new Mock<IBasketRepository>();
            _basketAggregate = new BasketAggregate(Id);
            _repositoryMock.Setup(r => r.Load(It.Is<string>(x => x == Id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_basketAggregate);

            _controller = new BasketControllerV1(_repositoryMock.Object);
        }

        [Fact]
        public async Task DispatchAddItemCommandToAggregate()
        {
            var result = await _controller.AddItem(Id, new AddItemCommand {Sku = "sku", Description = "desc"},
                CancellationToken.None);
            result.Should().BeOfType<OkResult>();

            _repositoryMock.Verify(r =>
                r.Save(It.Is<BasketAggregate>(a => a.Id == Id && a.Items.FirstOrDefault().Sku == "sku"),
                    It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task DispatchUpdateItemCommandToAggregate()
        {
            _basketAggregate.Apply(new AddItemCommand {Sku = "sku", Description = "desc"});

            var result = await _controller.UpdateItem(Id, new UpdateItemCommand() {Sku = "sku", Quantity = 3},
                CancellationToken.None);
            result.Should().BeOfType<OkResult>();

            _repositoryMock.Verify(r =>
                r.Save(It.Is<BasketAggregate>(a => a.Id == Id && a.Items.FirstOrDefault().Quantity == 3),
                    It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task DispatchRemoveItemCommandToAggregate()
        {
            _basketAggregate.Apply(new AddItemCommand {Sku = "sku", Description = "desc"});

            var result = await _controller.RemoveItem(Id, new RemoveItemCommand {Sku = "sku"},
                CancellationToken.None);
            result.Should().BeOfType<OkResult>();

            _repositoryMock.Verify(r => r.Save(It.Is<BasketAggregate>(a => a.Id == Id && a.Items.Count == 0),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task DispatchClearItemsCommandToAggregate()
        {
            _basketAggregate.Apply(new AddItemCommand {Sku = "sku", Description = "desc"});
            _basketAggregate.Apply(new AddItemCommand {Sku = "sku2", Description = "desc"});

            var result = await _controller.ClearItems(Id, CancellationToken.None);
            result.Should().BeOfType<OkResult>();

            _repositoryMock.Verify(r => r.Save(It.Is<BasketAggregate>(a => a.Id == Id && a.Items.Count == 0),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task ReturnBasketAsDto_WhenGetInvoked()
        {
            _basketAggregate.Apply(new AddItemCommand {Sku = "sku", Description = "desc"});

            var response = await _controller.Get(Id, CancellationToken.None);

            var result = response.Should().BeAssignableTo<ObjectResult>().Subject;
            var basketDto = result.Value.Should().BeOfType<BasketResponse>().Subject;
            basketDto.Id.Should().Be(Id);
            basketDto.Items.Should().ContainSingle(x => x.Description == "desc" && x.Sku == "sku" && x.Quantity == 1);
        }
    }
}