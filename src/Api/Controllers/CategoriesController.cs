using Application.Inventory.Categories.Commands.Create;
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
}
