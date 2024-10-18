using Domain.Invoices.Events;
using SharedKernel.Domain;

namespace Domain.Invoices;
public sealed class Transaction : BaseAuditableEntity
{
	public Guid InvoiceId { get; private set; }
	public Invoice Invoice { get; private set; } = null!;
	public decimal Amount { get; private set; }
	public PaymentMethodType PaymentMethod { get; private set; }
	public string? ReferenceNumber { get; private set; }
	public Currency Currency { get; private set; }

	public Transaction(Guid invoiceId, decimal amount, PaymentMethodType paymentMethod, string? referenceNumber, Currency currency)
	{
		InvoiceId = invoiceId;
		Amount = amount;
		PaymentMethod = paymentMethod;
		ReferenceNumber = referenceNumber;
		Currency = currency;
	}

	public static Transaction Create(Guid invoiceId, decimal amount, PaymentMethodType paymentMethod, string? referenceNumber, Currency? currency)
	{
		var transaction = new Transaction(
			invoiceId: invoiceId,
			amount: amount,
			paymentMethod: paymentMethod,
			referenceNumber: referenceNumber,
			currency: currency ?? Currency.DOP);

		transaction.RaiseEvent(new TransactionCreatedEvent(transaction));

		return transaction;
	}

	private Transaction() { }
}
