using Domain.Billing;
using SharedKernel.Contracts.Payments;
using SharedKernel.Interfaces.Messaging;

namespace Application.Billing.Payments.Commands.Make;
public record MakePaymentCommand(
	Guid InvoiceId,
	decimal Amount,
	PaymentMethod PaymentMethod,
	string? ReferenceNumber,
	Currency Currency
) : ICommand<PaymentResponse>;
