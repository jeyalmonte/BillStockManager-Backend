using Domain.Invoices.Events;
using Domain.Products;
using SharedKernel.Domain;
using SharedKernel.Results;

namespace Domain.Invoices;
public sealed class InvoiceDetail : BaseAuditableEntity
{
	public Guid InvoiceId { get; private set; }
	public Invoice Invoice { get; private set; } = null!;
	public Guid ProductId { get; private set; }
	public Product Product { get; private set; } = null!;
	public int Quantity { get; private set; }
	public decimal UnitPrice { get; private set; }
	public decimal SubTotal { get; private set; }
	public decimal? Discount { get; private set; }
	public InvoiceDetail(Guid invoiceId, Guid productId, int quantity, decimal unitPrice, decimal? discount)
	{
		InvoiceId = invoiceId;
		ProductId = productId;
		Quantity = quantity;
		UnitPrice = unitPrice;
		Discount = discount;
		SubTotal = CalculateSubTotal(quantity, unitPrice, discount);
	}

	public static Result<InvoiceDetail> Create(Guid invoiceId, Guid productId, int quantity, decimal unitPrice, decimal? discount)
	{
		if (quantity <= 0)
		{
			Error.Conflict(description: "Quantity must be greater than zero.");
		}

		if (unitPrice <= 0)
		{
			Error.Conflict(description: "Unit price must be greater than zero.");
		}

		if (discount.HasValue && discount.Value < 0)
		{
			Error.Conflict(description: "Discount cannot be negative.");
		}

		var invoiceDetail = new InvoiceDetail(
			invoiceId: invoiceId,
			productId: productId,
			quantity: quantity,
			unitPrice: unitPrice,
			discount: discount);

		invoiceDetail.RaiseEvent(new InvoiceDetailCreatedEvent(invoiceDetail));

		return invoiceDetail;
	}

	private static decimal CalculateSubTotal(int quantity, decimal unitPrice, decimal? discount)
	{
		var subtotal = quantity * unitPrice;
		return discount.HasValue ? subtotal - discount.Value : subtotal;
	}
	private InvoiceDetail() { }
}
