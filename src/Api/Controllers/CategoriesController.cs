using Application.Inventory.Categories.Commands.Create;
using Application.Inventory.Categories.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Contracts.Categories;
using SharedKernel.Results;

namespace Api.Controllers;
[Route("api/inventory/[controller]")]
public class CategoriesController : ApiController
{
	[HttpPost]
	[EndpointSummary("Create a category")]
	[ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
	{
		var result = await Sender.Send(command);

		return result.Match(
			category => Created("", category),
			Problem);
	}

	[HttpGet]
	[EndpointSummary("Get all categories")]
	[ProducesResponseType(typeof(ICollection<CategoryResponse>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAll()
	{
		var result = await Sender.Send(new GetAllCategoriesQuery());

		return result.Match(
			Ok,
			Problem);
	}
}
