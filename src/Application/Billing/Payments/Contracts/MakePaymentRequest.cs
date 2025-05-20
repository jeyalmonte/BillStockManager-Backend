namespace Application.Billing.Payments.Contracts;
public record MakePaymentRequest(
	decimal Amount,
	string PaymentMethod,
	string? ReferenceNumber,
	string Currency
	);
