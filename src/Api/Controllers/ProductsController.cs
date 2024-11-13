using Application.Inventory.Products.Commands.Create;
using Application.Inventory.Products.Queries.GetById;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Contracts.Products;
using SharedKernel.Results;

namespace Api.Controllers;
[Route("api/inventory/[controller]")]
public class ProductsController : ApiController
{
	[HttpPost]
	[EndpointSummary("Create a product")]
	[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
	{
		var result = await Sender.Send(command);

		return result.Match(
			product => CreatedAtAction(
				actionName: nameof(GetById),
				routeValues: new { productId = product.Id },
				value: product),
			Problem);
	}

	[HttpGet("{productId:guid}")]
	[EndpointSummary("Get a product by id")]
	[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById(Guid productId)
	{
		var result = await Sender.Send(new GetProductByIdQuery(productId));

		return result.Match(
			Ok,
			Problem);
	}
}
