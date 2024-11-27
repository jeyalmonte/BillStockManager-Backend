using Application.Billing.Invoices.Commands.Create;
using Application.Billing.Invoices.Queries.GetById;
using Application.Billing.Invoices.Queries.GetDetails;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Contracts.Invoices;
using SharedKernel.Results;

namespace Api.Controllers;
[Route("api/billing/[controller]")]
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
}
