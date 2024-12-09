using Domain.Billing.Events;
using SharedKernel.Domain;
using SharedKernel.Results;

namespace Domain.Billing;
public sealed class Payment : AuditableEntity
{
	public Guid InvoiceId { get; private set; }
	public Invoice Invoice { get; private set; } = null!;
	public decimal Amount { get; private set; }
	public PaymentMethod PaymentMethod { get; private set; }
	public string? ReferenceNumber { get; private set; }
	public Currency Currency { get; private set; }

	private Payment(Guid invoiceId, decimal amount, PaymentMethod paymentMethod, string? referenceNumber, Currency currency)
	{
		InvoiceId = invoiceId;
		Amount = amount;
		PaymentMethod = paymentMethod;
		ReferenceNumber = referenceNumber;
		Currency = currency;
	}

	public static Result<Payment> Create(Guid invoiceId, decimal amount, PaymentMethod paymentMethod, string? referenceNumber, Currency? currency)
	{
		if (amount <= 0)
		{
			return Error.Conflict(description: "Amount must be greater than zero.");
		}

		if (RequiresReference(paymentMethod) && string.IsNullOrEmpty(referenceNumber))
		{
			return Error.Conflict("Reference number is required for this payment method.");
		}

		var transaction = new Payment(
			invoiceId: invoiceId,
			amount: amount,
			paymentMethod: paymentMethod,
			referenceNumber: referenceNumber,
			currency: currency ?? Currency.DOP);

		transaction.RaiseEvent(new PaymentCreatedDomainEvent(transaction));

		return transaction;
	}

	private static bool RequiresReference(PaymentMethod paymentMethod) => paymentMethod switch
	{
		PaymentMethod.Card => true,
		PaymentMethod.Transfer => true,
		_ => false
	};

	private Payment() { }
}
