namespace SharedKernel.Contracts.Invoices;

public record InvoiceDetailResponse(
	Guid Id,
	ProductInvoiceDto Product,
	int Quantity,
	decimal? Discount,
	decimal SubTotal
	);



