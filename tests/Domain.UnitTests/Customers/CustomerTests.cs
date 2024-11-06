using Domain.Customers;
using Domain.Invoices;
using FluentAssertions;

namespace Domain.UnitTests.Customers;
public class CustomerTests
{
	[Fact]
	public void Create_ShouldReturnCustomerInstance()
	{
		// Arrange
		var customer = Customer.NewBuilder()
			.WithDocumentType(DocumentType.Cedula)
			.WithDocument("12345678901")
			.WithFullName("Test")
			.WithGender(GenderType.Male)
			.WithEmail("test@test.com")
			.Build();

		// Assert
		customer.Should().NotBeNull();
		customer.Should().BeOfType<Customer>();
	}

	[Fact]
	public void Update_ShouldUpdateCustomer()
	{
		// Arrange
		var customer = Customer.NewBuilder()
			.WithDocumentType(DocumentType.Cedula)
			.WithDocument("12345678901")
			.WithFullName("Test")
			.WithGender(GenderType.Male)
			.WithEmail("test@test.com")
			.Build();

		// Act
		customer.Update(
			fullName: "Test Updated",
			nickname: null,
			email: "test@test.com",
			phoneNumber: null,
			address: null);

		// Assert

		customer.FullName.Should().NotBe("Test");
		customer.FullName.Should().Be("Test Updated");
	}

	[Fact]
	public void AddInvoice_WhenInvoiceDoesNotBelongToCustomer_ShouldReturnConflict()
	{
		// Arrange
		var customer = Customer.NewBuilder()
			 .WithDocumentType(DocumentType.Cedula)
			 .WithDocument("12345678901")
			 .WithFullName("Test")
			 .WithGender(GenderType.Male)
			 .WithEmail("test@test.com")
			 .Build();

		var invoice = Invoice.Create(Guid.NewGuid());
		invoice.Should().NotBeNull();

		// Act
		var result = customer.AddInvoice(invoice);
		result.Should().NotBeNull();
		result.HasError.Should().BeTrue();
	}

	[Fact]
	public void AddInvoice_ShouldAddInvoiceToCustomer()
	{
		// Arrange
		var customer = Customer.NewBuilder()
			.WithDocumentType(DocumentType.Cedula)
			.WithDocument("12345678901")
			.WithFullName("Test")
			.WithGender(GenderType.Male)
			.WithEmail("test@test.com")
			.Build();

		var invoice = Invoice.Create(customer.Id);
		invoice.Should().NotBeNull();

		// Act
		var result = customer.AddInvoice(invoice);
		result.Should().NotBeNull();
		result.HasError.Should().BeFalse();
	}
}
