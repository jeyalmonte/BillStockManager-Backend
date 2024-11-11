using Application.Common.Models;
using Application.Customers.Commands.Create;
using Application.Customers.Commands.Update;
using Application.Customers.Queries.GetByFilter;
using Application.Customers.Queries.GetById;
using Domain.Customers;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Contracts.Customers;
using SharedKernel.Results;

namespace Api.Controllers;
[Route("api/[controller]")]
public class CustomersController : ApiController
{
	[HttpPost]
	[EndpointSummary("Create a customer")]
	[ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command)
	{
		var result = await Sender.Send(command);

		return result.Match(
			customer => CreatedAtAction(
				actionName: nameof(GetById),
				routeValues: new { customerId = customer.Id },
				value: customer),
		  Problem);
	}

	[HttpGet("{customerId:guid}")]
	[EndpointSummary("Get customer by id")]
	[ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById(Guid customerId)
	{
		var result = await Sender.Send(new GetCustomerByIdQuery(customerId));
		return result.Match(Ok, Problem);
	}

	[HttpGet]
	[EndpointSummary("Get customers by filter")]
	[ProducesResponseType(typeof(PaginatedResult<CustomerResponse>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetByFilter([FromQuery] GetCustomersByFilterQuery query)
	{
		var result = await Sender.Send(query);
		return result.Match(
			response => Ok(response),
			Problem);
	}

	[HttpPut("{customerId:guid}")]
	[EndpointSummary("Update a customer")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(Guid customerId, [FromBody] UpdateCustomerRequest request)
	{
		if (!Enum.TryParse<GenderType>(request.Gender, out var gender))
		{
			return Problem(
				statusCode: StatusCodes.Status400BadRequest,
				detail: "Invalid gender type");
		}

		var query = new UpdateCustomerCommand(
			Id: customerId,
			FullName: request.FullName,
			Nickname: request.Nickname,
			Gender: gender,
			Email: request.Email,
			PhoneNumber: request.PhoneNumber,
			Address: request.Address
			);

		var result = await Sender.Send(query);

		return result.Match(
			_ => NoContent(),
			Problem);
	}
}
