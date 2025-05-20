using Application.Billing.Invoices.Commands.Cancel;
using Application.Billing.Invoices.Commands.Create;
using Application.Billing.Invoices.Contracts;
using Application.Billing.Invoices.Queries.GetByFilter;
using Application.Billing.Invoices.Queries.GetById;
using Application.Billing.Invoices.Queries.GetDetails;
using Application.Common.Results;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Results;

namespace Api.Controllers.V1;
[Route("billing/[controller]")]
public class InvoicesController : ApiController
{
	[HttpPost]
	[EndpointSummary("Create a invoice")]
	[ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Create([FromBody] CreateInvoiceCommand command)
	{
		var result = await Sender.Send(command);
		return result.Match(
			invoice => CreatedAtAction(
				actionName: nameof(GetById),
				routeValues: new { invoiceId = invoice.Id },
				value: invoice),
			Problem);
	}

	[HttpGet("{invoiceId:guid}")]
	[EndpointSummary("Get a invoice by id")]
	[ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById(Guid invoiceId)
	{
		var result = await Sender.Send(new GetInvoiceByIdQuery(invoiceId));
		return result.Match(Ok, Problem);
	}

	[HttpGet("{invoiceId:guid}/details")]
	[EndpointSummary("Get details by invoice id")]
	[ProducesResponseType(typeof(List<InvoiceDetailResponse>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetDetails(Guid invoiceId)
	{
		var result = await Sender.Send(new GetInvoiceDetailsQuery(invoiceId));
		return result.Match(Ok, Problem);
	}

	[HttpGet]
	[EndpointSummary("Get invoices by filter")]
	[ProducesResponseType(typeof(PaginatedResult<InvoiceResponse>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetByFilter([FromQuery] GetInvoicesByFilterQuery query)
	{
		var result = await Sender.Send(query);
		return result.Match(
			invoicesPaginated => Ok(invoicesPaginated),
			Problem);
	}

	[HttpDelete("{invoiceId:guid}")]
	[EndpointSummary("Cancel a invoice")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Cancel(Guid invoiceId)
	{
		var result = await Sender.Send(new CancelInvoiceCommand(invoiceId));
		return result.Match(
			_ => NoContent(),
			Problem);
	}
}
