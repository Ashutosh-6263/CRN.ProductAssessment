using Asp.Versioning;
using CRN.Application.DTOs.Item;
using CRN.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRN.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/items")]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        // GET: api/v1.0/items/1
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken cancellationToken = default)
        {
            var item = await _itemService.GetByIdAsync(
                id,
                cancellationToken);

            if (item == null)
            {
                return NotFound(new
                {
                    message = $"Item with id {id} was not found."
                });
            }

            return Ok(item);
        }

        // POST: api/v1.0/items
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(
            [FromBody] CreateItemRequest request,
            CancellationToken cancellationToken = default)
        {
            var item = await _itemService.CreateAsync(
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = item.Id,
                    version = "1.0"
                },
                item);
        }

        // PUT: api/v1.0/items/1
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateItemRequest request,
            CancellationToken cancellationToken = default)
        {
            var item = await _itemService.UpdateAsync(
                id,
                request,
                cancellationToken);

            if (item == null)
            {
                return NotFound(new
                {
                    message = $"Item with id {id} was not found."
                });
            }

            return Ok(item);
        }

        // DELETE: api/v1.0/items/1
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken = default)
        {
            var deleted = await _itemService.DeleteAsync(
                id,
                cancellationToken);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = $"Item with id {id} was not found."
                });
            }

            return NoContent();
        }

    }
}
