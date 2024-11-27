namespace SharedKernel.Contracts.Invoices;

public record ProductInvoiceDto(
	Guid Id,
	string Name,
	decimal Price,
	decimal? Discount
	);



