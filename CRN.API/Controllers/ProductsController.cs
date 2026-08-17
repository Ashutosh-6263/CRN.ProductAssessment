using Asp.Versioning;
using CRN.Application.DTOs.Product;
using CRN.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CRN.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IItemService _itemService;

        public ProductsController(IProductService productService, IItemService itemService)
        {
            _productService = productService;
            _itemService = itemService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _productService.GetAllAsync(
                pageNumber,
                pageSize,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken cancellationToken = default)
        {
            var product = await _productService.GetByIdAsync(
                id,
                cancellationToken);

            if (product == null)
            {
                return NotFound(new
                {
                    message = $"Product with id {id} was not found."
                });
            }

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductRequest request,
            CancellationToken cancellationToken = default)
        {
            var username = User.Identity?.Name ?? "system";

            var product = await _productService.CreateAsync(
                request,
                username,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = product.Id,
                    version = "1.0"
                },
                product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateProductRequest request,
            CancellationToken cancellationToken = default)
        {
            var username = User.Identity?.Name ?? "system";

            var product = await _productService.UpdateAsync(
                id,
                request,
                username,
                cancellationToken);

            if (product == null)
            {
                return NotFound(new
                {
                    message = $"Product with id {id} was not found."
                });
            }

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken = default)
        {
            var deleted = await _productService.DeleteAsync(
                id,
                cancellationToken);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = $"Product with id {id} was not found."
                });
            }

            return NoContent();
        }
        [HttpGet("{productId:int}/items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetItemsByProductId(
    int productId,
    CancellationToken cancellationToken = default)
        {
            var items = await _itemService.GetByProductIdAsync(
                productId,
                cancellationToken);

            return Ok(items);
        }
    }
}
