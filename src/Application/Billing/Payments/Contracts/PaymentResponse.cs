namespace Application.Billing.Payments.Contracts;
public record PaymentResponse(
	Guid Id,
	decimal Amount,
	string PaymentMethod,
	string? ReferenceNumber,
	string Currency
	);
