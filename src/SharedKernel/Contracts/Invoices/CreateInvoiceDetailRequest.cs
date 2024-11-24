namespace SharedKernel.Contracts.Invoices;
public record CreateInvoiceDetailRequest(
	Guid ProductId,
	int Quantity,
	decimal? Discount);
