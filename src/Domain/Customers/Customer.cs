using Domain.Customers.Events;
using Domain.Invoices;
using SharedKernel.Domain;
using SharedKernel.Results;

namespace Domain.Customers;
public sealed class Customer : BaseAuditableEntity
{
	private readonly List<Invoice> _invoices = [];
	public string FullName { get; private set; } = null!;
	public string? Nickname { get; private set; }
	public DocumentType DocumentType { get; private set; }
	public string Document { get; private set; } = null!;
	public GenderType Gender { get; private set; }
	public string? Email { get; private set; }
	public string? PhoneNumber { get; private set; }
	public string? Address { get; private set; }
	public IReadOnlyList<Invoice> Invoices => _invoices.AsReadOnly();

	public void Update(string fullName, string? nickname, string? email, string? phoneNumber, string? address)
	{
		FullName = fullName;
		Nickname = nickname;
		Email = email;
		PhoneNumber = phoneNumber;
		Address = address;

		RaiseEvent(new CustomerUpdatedDomainEvent(Id));
	}

	public Result<Success> AddInvoice(Invoice invoice)
	{
		if (invoice.CustomerId != Id)
		{
			return Error.Conflict("Invoice does not belong to this customer.");
		}
		_invoices.Add(invoice);

		return Result.Success;
	}

	public static Builder NewBuilder() => new();
	private Customer() { }
	public sealed class Builder
	{
		private readonly Customer _customer = new();

		public Builder WithFullName(string fullName)
		{
			_customer.FullName = fullName;
			return this;
		}

		public Builder WithNickname(string? nickname)
		{
			_customer.Nickname = nickname;
			return this;
		}

		public Builder WithDocumentType(DocumentType documentType)
		{
			_customer.DocumentType = documentType;
			return this;
		}

		public Builder WithDocument(string document)
		{
			_customer.Document = document;
			return this;
		}

		public Builder WithGender(GenderType gender)
		{
			_customer.Gender = gender;
			return this;
		}

		public Builder WithEmail(string? email)
		{
			_customer.Email = email;
			return this;
		}

		public Builder WithPhoneNumber(string? phoneNumber)
		{
			_customer.PhoneNumber = phoneNumber;
			return this;
		}

		public Builder WithAddress(string? address)
		{
			_customer.Address = address;
			return this;
		}

		public Customer Build()
		{
			_customer.RaiseEvent(new CustomerCreatedDomainEvent(_customer));
			return _customer;
		}
	}
}



