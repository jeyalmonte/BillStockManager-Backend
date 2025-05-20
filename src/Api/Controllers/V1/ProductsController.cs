using Application.Common.Results;
using Application.Inventory.Products.Commands.Create;
using Application.Inventory.Products.Commands.Delete;
using Application.Inventory.Products.Commands.Update;
using Application.Inventory.Products.Commands.UpdateStock;
using Application.Inventory.Products.Contracts;
using Application.Inventory.Products.Queries.GetByFilter;
using Application.Inventory.Products.Queries.GetById;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Results;

namespace Api.Controllers.V1;
[Route("inventory/[controller]")]
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

	[HttpGet]
	[EndpointSummary("Get products by filter")]
	[ProducesResponseType(typeof(PaginatedResult<ProductResponse>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetByFilter([FromQuery] GetProductsByFilterQuery query)
	{
		var result = await Sender.Send(query);
		return result.Match(
			response => Ok(response),
			Problem);
	}

	[HttpPut("{productId:guid}")]
	[EndpointSummary("Update a product")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Update(Guid productId, [FromBody] UpdateProductRequest request)
	{
		var command = new UpdateProductCommand(
			Id: productId,
			Name: request.Name,
			Description: request.Description,
			CategoryId: request.CategoryId,
			Price: request.Price,
			Discount: request.Discount);

		var result = await Sender.Send(command);

		return result.Match(
			_ => NoContent(),
			Problem);
	}

	[HttpPatch("{productId:guid}/stock")]
	[EndpointSummary("Increase or reduce product stock")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> UpdateStock(Guid productId, [FromBody] UpdateProductStockRequest request)
	{
		var command = new UpdateProductStockCommand(
			ProductId: productId,
			Quantity: request.Quantity);

		var result = await Sender.Send(command);

		return result.Match(
			_ => NoContent(),
			Problem);
	}

	[HttpDelete("{productId:guid}")]
	[EndpointSummary("Delete a product")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(Guid productId)
	{
		var result = await Sender.Send(new DeleteProductCommand(productId));

		return result.Match(
			_ => NoContent(),
			Problem);
	}
}
