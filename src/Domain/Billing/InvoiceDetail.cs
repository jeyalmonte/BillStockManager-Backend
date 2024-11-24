using Domain.Inventory;
using SharedKernel.Domain;
using SharedKernel.Results;

namespace Domain.Billing;
public sealed class InvoiceDetail : BaseAuditableEntity
{
	public Guid InvoiceId { get; private set; }
	public Invoice Invoice { get; private set; } = null!;
	public Guid ProductId { get; private set; }
	public Product Product { get; private set; } = null!;
	public int Quantity { get; private set; }
	public decimal UnitPrice { get; private set; }
	public decimal? Discount { get; private set; }
	public decimal SubTotal => CalculateSubTotal();
	public InvoiceDetail(Guid invoiceId, Guid productId, int quantity, decimal unitPrice, decimal? discount)
	{
		InvoiceId = invoiceId;
		ProductId = productId;
		Quantity = quantity;
		UnitPrice = unitPrice;
		Discount = discount;
	}

	public static Result<InvoiceDetail> Create(Guid invoiceId, Product product, int quantity, decimal? discount = 0)
	{
		if (quantity <= 0)
		{
			return Error.Conflict(description: "Quantity must be greater than zero.");
		}

		if (product.Price <= 0)
		{
			return Error.Conflict(description: "Unit price must be greater than zero.");
		}

		if (discount.HasValue && discount.Value < 0)
		{
			return Error.Conflict(description: "Discount cannot be negative.");
		}

		var invoiceDetail = new InvoiceDetail(
			invoiceId: invoiceId,
			productId: product.Id,
			quantity: quantity,
			unitPrice: product.GetPriceAfterDiscount(),
			discount: discount);

		return invoiceDetail;
	}

	private decimal CalculateSubTotal()
	{
		var subtotal = Quantity * UnitPrice;
		return Discount.HasValue
			? subtotal - (subtotal * (Discount.Value / 100))
			: subtotal;
	}
	private InvoiceDetail() { }
}
