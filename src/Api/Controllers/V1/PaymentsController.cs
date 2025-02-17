using Application.Billing.Payments.Commands.Make;
using Domain.Billing;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Contracts.Payments;
using SharedKernel.Results;

namespace Api.Controllers.V1;
[Route("billing/Invoices/{invoiceId:guid}/[controller]")]
[ApiController]
public class PaymentsController : ApiController
{
	[HttpPost]
	public async Task<IActionResult> MakePayment(Guid invoiceId, MakePaymentRequest request)
	{
		var paymentMethod = Enum.Parse<PaymentMethod>(request.PaymentMethod, true);
		var currency = Enum.Parse<Currency>(request.Currency, true);

		var command = new MakePaymentCommand(
			InvoiceId: invoiceId,
			Amount: request.Amount,
			PaymentMethod: paymentMethod,
			ReferenceNumber: request.ReferenceNumber,
			Currency: currency
		);

		var result = await Sender.Send(command);

		return result.Match(Ok, Problem);
	}
}
