using Application.Inventory.Products.Commands.Create;
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
            product => Created("", product),
            Problem);
    }
}
