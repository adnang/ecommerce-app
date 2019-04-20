using System.Threading;
using System.Threading.Tasks;
using ECommerceApp.Api.Domain.Commands;
using ECommerceApp.Api.Domain.Interfaces;
using ECommerceApp.Api.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Api.Controllers
{
    [Route("api/basket")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
        {
            var basketAggregate = await _repository.Load(id, cancellationToken);
            return Ok(basketAggregate.MapToDto());
        }

        [HttpPut("{id}/items/add")]
        public async Task<IActionResult> AddItem(string id, [FromBody] AddItemCommand command, CancellationToken cancellationToken)
        {
            var basketAggregate = await _repository.Load(id, cancellationToken);
            basketAggregate.Apply(command);
            await _repository.Save(basketAggregate, cancellationToken);

            return Ok();
        }

        [HttpPut("{id}/items/update")]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] UpdateItemCommand command, CancellationToken cancellationToken)
        {
            var basketAggregate = await _repository.Load(id, cancellationToken);
            basketAggregate.Apply(command);
            await _repository.Save(basketAggregate, cancellationToken);

            return Ok();
        }

        [HttpPut("{id}/items/remove")]
        public async Task<IActionResult> RemoveItem(string id, [FromBody] RemoveItemCommand command, CancellationToken cancellationToken)
        {
            var basketAggregate = await _repository.Load(id, cancellationToken);
            basketAggregate.Apply(command);
            await _repository.Save(basketAggregate, cancellationToken);

            return Ok();            
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> ClearItems(string id, CancellationToken cancellationToken)
        {
            var basketAggregate = await _repository.Load(id, cancellationToken);
            basketAggregate.Apply(new ClearItemsCommand());
            await _repository.Save(basketAggregate, cancellationToken);

            return Ok();            
        }
    }
}