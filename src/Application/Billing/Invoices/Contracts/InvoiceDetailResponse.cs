namespace Application.Billing.Invoices.Contracts;

public record InvoiceDetailResponse(
	Guid Id,
	ProductInvoiceDto Product,
	int Quantity,
	decimal? Discount,
	decimal SubTotal
	);

public record ProductInvoiceDto(
	Guid Id,
	string Name,
	decimal Price,
	decimal? Discount
	);



