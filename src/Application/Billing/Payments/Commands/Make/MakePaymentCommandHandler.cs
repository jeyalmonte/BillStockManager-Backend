using Application.Billing.Payments.Contracts;
using Domain.Billing;
using Domain.Billing.Repositories;
using Mapster;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Billing.Payments.Commands.Make;
public class MakePaymentCommandHandler(
	IInvoiceRepository invoiceRepository,
	IUnitOfWork unitOfWork
	) : ICommandHandler<MakePaymentCommand, PaymentResponse>
{
	public async Task<Result<PaymentResponse>> Handle(MakePaymentCommand request, CancellationToken cancellationToken)
	{
		var invoice = await invoiceRepository.GetByIdAsync(
			id: request.InvoiceId,
			asNoTracking: false,
			cancellationToken: cancellationToken
		);

		if (invoice is null)
		{
			return Error.NotFound(description: "Invoice not found.");
		}

		if (invoice.Status == InvoiceStatus.Paid)
		{
			return Error.Failure(description: "Invoice is already paid.");
		}

		var payment = Payment.Create(
			invoiceId: invoice.Id,
			amount: request.Amount,
			paymentMethod: request.PaymentMethod,
			referenceNumber: request.ReferenceNumber,
			currency: request.Currency
		);

		if (payment.HasError)
		{
			return payment.Errors;
		}

		var paymentProcessingResult = invoice.ProcessPayment(payment.Value);

		if (paymentProcessingResult.HasError)
		{
			return paymentProcessingResult.Errors;
		}

		await unitOfWork.CommitAsync(cancellationToken);

		return payment.Value.Adapt<PaymentResponse>();
	}
}
