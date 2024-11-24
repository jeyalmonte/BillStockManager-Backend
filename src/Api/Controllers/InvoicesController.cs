using Application.Billing.Invoices.Commands.Create;
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
	public async Task<IActionResult> GetById(Guid invoiceId)
	{
		await Task.CompletedTask;
		return Ok();

	}

}
