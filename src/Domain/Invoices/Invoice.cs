using Domain.Customers;
using Domain.Invoices.Events;
using SharedKernel.Domain;
using SharedKernel.Results;

namespace Domain.Invoices;
public sealed class Invoice : BaseAuditableEntity
{
	private readonly List<InvoiceDetail> _invoiceDetails = [];
	private readonly List<Transaction> _transactions = [];
	public Guid CustomerId { get; private set; }
	public Customer Customer { get; private set; } = null!;
	public decimal TotalAmount { get; private set; }
	public InvoiceStatus Status { get; private set; }
	public IReadOnlyList<InvoiceDetail> InvoiceDetails => _invoiceDetails.AsReadOnly();
	public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();

	public Invoice(Guid customerId)
	{
		CustomerId = customerId;
		TotalAmount = 0;
		Status = InvoiceStatus.Pending;
	}

	public static Invoice Create(Guid customerId)
	{
		var invoice = new Invoice(customerId: customerId);

		invoice.RaiseEvent(new InvoiceCreatedEvent(invoice));

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

		return Result.Success;
	}

	public Result<Success> AddTransaction(Transaction transaction)
	{
		if (transaction.Amount <= 0)
		{
			return Error.Conflict(description: "Amount must be greater than zero.");
		}

		decimal outstandingBalance = CalculateOutstandingBalance();

		if (transaction.Amount > outstandingBalance)
		{
			return Error.Conflict(description: _transactions.Count != 0
				? "Amount exceeds the outstanding balance."
				: "Amount exceeds the total amount.");
		}

		outstandingBalance -= transaction.Amount;

		if (outstandingBalance == 0)
		{
			MarkAsPaid();
		}

		_transactions.Add(transaction);
		return Result.Success;
	}

	public Result<Success> MarkAsPaid()
	{
		if (Status == InvoiceStatus.Paid)
		{
			return Error.Conflict(description: "Invoice is already paid");
		}

		if (Status == InvoiceStatus.Cancelled)
		{
			return Error.Conflict(description: "Cancelled invoice cannot be marked as paid.");
		}

		Status = InvoiceStatus.Paid;

		RaiseEvent(new InvoicePaidEvent(Id));

		return Result.Success;
	}

	public Result<Success> MarkAsCancelled()
	{
		if (Status == InvoiceStatus.Cancelled)
		{
			return Error.Conflict(description: "Invoice is already paid");
		}

		Status = InvoiceStatus.Cancelled;
		RaiseEvent(new InvoiceCancelledEvent(Id));

		return Result.Success;
	}

	private decimal CalculateOutstandingBalance()
	{
		var totalPaid = _transactions.Sum(t => t.Amount);
		return TotalAmount - totalPaid;
	}

	private Invoice() { }
}
