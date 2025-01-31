using Domain.Billing.Events;
using Domain.Customers;
using SharedKernel.Domain;
using SharedKernel.Results;

namespace Domain.Billing;
public sealed class Invoice : AuditableEntity
{
	private readonly List<InvoiceDetail> _invoiceDetails = [];
	private readonly List<Payment> _payments = [];
	public int InvoiceNumber { get; private set; }
	public Guid CustomerId { get; private set; }
	public Customer Customer { get; private set; } = null!;
	public decimal TotalAmount { get; private set; }
	public decimal PaidAmount { get; private set; }
	public decimal OutstandingAmount => TotalAmount - PaidAmount;
	public InvoiceStatus Status { get; private set; }
	public IReadOnlyList<InvoiceDetail> InvoiceDetails => _invoiceDetails.AsReadOnly();
	public IReadOnlyList<Payment> Payments => _payments.AsReadOnly();

	private Invoice(Guid customerId)
	{
		CustomerId = customerId;
		TotalAmount = 0;
		Status = InvoiceStatus.Pending;
	}

	public static Invoice Create(Guid customerId)
	{
		var invoice = new Invoice(customerId: customerId);

		invoice.RaiseEvent(new InvoiceCreatedDomainEvent(invoice));

		return invoice;
	}

	public Result<Success> AddInvoiceDetail(InvoiceDetail invoiceDetail)
	{
		if (_invoiceDetails.Any(d => d.ProductId == invoiceDetail.ProductId))
		{
			return Error.Conflict(description: $"Product {invoiceDetail.ProductId} already exists in the invoice.");
		}

		_invoiceDetails.Add(invoiceDetail);
		TotalAmount += invoiceDetail.SubTotal;

		RaiseEvent(new InvoiceDetailAddedDomainEvent(invoiceDetail));

		return Result.Success;
	}

	public Result<Success> ProcessPayment(Payment payment)
	{

		if (payment.Amount > OutstandingAmount)
		{
			return Error.Conflict(description: _payments.Count != 0
				? "Amount exceeds the outstanding balance."
				: "Amount exceeds the total amount.");
		}

		PaidAmount += payment.Amount;

		if (OutstandingAmount == 0)
		{
			RaiseEvent(new InvoicePaidDomainEvent(Id));
		}

		_payments.Add(payment);

		return Result.Success;
	}

	public Result<Success> MarkAsPaid()
	{
		if (Status == InvoiceStatus.Paid)
		{
			return Error.Conflict(description: "Invoice is already paid.");
		}

		if (Status == InvoiceStatus.Cancelled)
		{
			return Error.Conflict(description: "Cancelled invoice cannot be marked as paid.");
		}

		Status = InvoiceStatus.Paid;

		return Result.Success;
	}

	public Result<Success> MarkAsCancelled()
	{
		if (Status == InvoiceStatus.Paid)
		{
			return Error.Conflict(description: "Invoice is already paid.");
		}

		Status = InvoiceStatus.Cancelled;
		RaiseEvent(new InvoiceCancelledDomainEvent(Id));

		return Result.Success;
	}
	private Invoice() { }
}
