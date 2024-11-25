namespace SharedKernel.Contracts.Invoices;

public record CustomerInvoiceDto(
	Guid Id,
	string FullName,
	string? Email
	);



