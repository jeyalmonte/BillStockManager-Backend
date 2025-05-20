namespace Application.Billing.Invoices.Contracts;
public record CreateInvoiceDetailRequest(
	Guid ProductId,
	int Quantity,
	decimal? Discount);
