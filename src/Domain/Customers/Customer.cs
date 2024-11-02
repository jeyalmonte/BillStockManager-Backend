using Domain.Customers.Events;
using Domain.Invoices;
using SharedKernel.Domain;
using SharedKernel.Results;

namespace Domain.Customers;
public sealed class Customer : BaseAuditableEntity
{
	private readonly List<Invoice> _invoices = [];
	public string FullName { get; private set; } = null!;
	public string? Nickname { get; set; }
	public GenderType Gender { get; private set; }
	public string? Email { get; private set; }
	public string? PhoneNumber { get; private set; }
	public string? Address { get; private set; }
	public IReadOnlyList<Invoice> Invoices => _invoices.AsReadOnly();

	public Customer(string fullName, string? nickname, GenderType gender, string email, string? phoneNumber, string? address)
	{
		FullName = fullName;
		Nickname = nickname;
		Gender = gender;
		Email = email;
		PhoneNumber = phoneNumber;
		Address = address;
	}

	public static Customer Create(string fullName, string? nickname, GenderType gender, string email, string? phoneNumber, string? address)
	{
		var customer = new Customer(
			fullName: fullName,
			nickname: nickname,
			gender: gender,
			email: email,
			phoneNumber: phoneNumber,
			address: address);

		customer.RaiseEvent(new CustomerCreatedEvent(customer));

		return customer;
	}

	public void Update(string fullName, string? nickname, string email, string? phoneNumber, string? address)
	{
		FullName = fullName;
		Nickname = nickname;
		Email = email;
		PhoneNumber = phoneNumber;
		Address = address;

		RaiseEvent(new CustomerUpdatedEvent(Id));
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

	private Customer() { }
}



