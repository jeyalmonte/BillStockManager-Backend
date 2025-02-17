namespace SharedKernel.Contracts.Payments;
public record MakePaymentRequest(
	decimal Amount,
	string PaymentMethod,
	string? ReferenceNumber,
	string Currency
	);
