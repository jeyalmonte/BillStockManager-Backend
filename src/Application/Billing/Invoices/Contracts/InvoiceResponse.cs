namespace Application.Billing.Invoices.Contracts;

public record InvoiceResponse
{
	public Guid Id { get; init; }
	public int InvoiceNumber { get; init; }
	public string InvoiceCode => $"INV-{InvoiceNumber:D6}";
	public decimal TotalAmount { get; init; }
	public decimal OutstandingAmount { get; init; }
	public CustomerInvoiceDto Customer { get; init; } = null!;
	public DateTime InvoiceDate { get; init; }
	public string Status { get; init; } = null!;
}

public record CustomerInvoiceDto(
	Guid Id,
	string FullName,
	string? Email
	);



