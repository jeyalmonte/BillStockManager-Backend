namespace SharedKernel.Contracts.Payments;
public record PaymentResponse(
	Guid Id,
	decimal Amount,
	string PaymentMethod,
	string? ReferenceNumber,
	string Currency
	);
