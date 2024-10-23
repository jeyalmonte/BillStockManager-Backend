using Domain.Invoices;
using Domain.Products;
using FluentAssertions;
using SharedKernel.Results;

namespace Domain.UnitTests.Invoices;
public class InvoiceTests
{
	[Fact]
	public void Create_ShouldReturnAnInvoiceInstance()
	{
		// Act
		var invoice = Invoice.Create(customerId: Guid.NewGuid());

		// Assert
		invoice.Should().NotBeNull();
		invoice.Should().BeOfType<Invoice>();
	}

	[Fact]
	public void AddInvoiceDetail_ShouldReturnConflict_WhenAddingDuplicateProducts()
	{
		// Arrange
		var invoice = Invoice.Create(customerId: Guid.NewGuid());
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		var invoiceDetail = InvoiceDetail.Create(
			invoiceId: invoice.Id,
			product: product,
			quantity: 100
		);

		var firstAddResult = invoice.AddInvoiceDetail(invoiceDetail.Value);
		firstAddResult.HasError.Should().BeFalse();

		// Act
		var secondAddResult = invoice.AddInvoiceDetail(invoiceDetail.Value);

		// Assert
		secondAddResult.HasError.Should().BeTrue();
		secondAddResult.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void AddInvoiceDetail_ShouldAddInvoiceDetail_WhenAddingUniqueProducts()
	{
		// Arrange
		var invoice = Invoice.Create(customerId: Guid.NewGuid());
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		var invoiceDetail = InvoiceDetail.Create(
			invoiceId: invoice.Id,
			product: product,
			quantity: 100
		);

		// Act
		var result = invoice.AddInvoiceDetail(invoiceDetail.Value);

		// Assert
		result.HasError.Should().BeFalse();
		invoice.InvoiceDetails.Should().Contain(invoiceDetail.Value);
	}

	[Fact]
	public void ProcessTransaction_ShouldReturnConflict_WhenAmountExceedsOutstandingBalance()
	{
		// Arrange
		var invoice = Invoice.Create(customerId: Guid.NewGuid());
		var transaction = Transaction.Create(
			invoiceId: invoice.Id,
			amount: 100,
			paymentMethod: PaymentMethodType.Cash,
			referenceNumber: null,
			currency: Currency.DOP
		);

		// Act
		var result = invoice.ProcessTransaction(transaction.Value);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void ProcessTransaction_ShouldReturnConflict_WhenAmountExceedsTotalAmount()
	{
		// Arrange
		var invoice = Invoice.Create(customerId: Guid.NewGuid());
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		var invoiceDetail = InvoiceDetail.Create(
			invoiceId: invoice.Id,
			product: product,
			quantity: 1
		);

		invoice.AddInvoiceDetail(invoiceDetail.Value);

		var transaction = Transaction.Create(
			invoiceId: invoice.Id,
			amount: 200,
			paymentMethod: PaymentMethodType.Cash,
			referenceNumber: null,
			currency: Currency.DOP
		);

		// Act
		var result = invoice.ProcessTransaction(transaction.Value);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void ProcessTransaction_ShouldReturnSuccess()
	{
		// Arrange
		var invoice = Invoice.Create(customerId: Guid.NewGuid());
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		var invoiceDetail = InvoiceDetail.Create(
			invoiceId: invoice.Id,
			product: product,
			quantity: 1
		);

		invoice.AddInvoiceDetail(invoiceDetail.Value);

		var transaction = Transaction.Create(
			invoiceId: invoice.Id,
			amount: 100,
			paymentMethod: PaymentMethodType.Cash,
			referenceNumber: null,
			currency: Currency.DOP
		);

		// Act
		var result = invoice.ProcessTransaction(transaction.Value);

		// Assert
		result.HasError.Should().BeFalse();
		invoice.Transactions.Should().Contain(transaction.Value);
	}

	[Fact]
	public void MarkAsPaid_ShouldReturnConflict_WhenInvoiceIsAlreadyPaid()
	{
		// Arrange
		var invoice = Invoice.Create(customerId: Guid.NewGuid());
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		var invoiceDetail = InvoiceDetail.Create(
			invoiceId: invoice.Id,
			product: product,
			quantity: 1
		);

		invoice.AddInvoiceDetail(invoiceDetail.Value);

		var transaction = Transaction.Create(
			invoiceId: invoice.Id,
			amount: 100,
			paymentMethod: PaymentMethodType.Cash,
			referenceNumber: null,
			currency: Currency.DOP
		);

		invoice.ProcessTransaction(transaction.Value);
		invoice.MarkAsPaid();

		// Act
		var result = invoice.MarkAsPaid();

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void MarkAsPaid_ShouldMarkInvoiceAsPaid()
	{
		// Arrange
		var invoice = Invoice.Create(customerId: Guid.NewGuid());
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		var invoiceDetail = InvoiceDetail.Create(
			invoiceId: invoice.Id,
			product: product,
			quantity: 1
		);

		invoice.AddInvoiceDetail(invoiceDetail.Value);

		var transaction = Transaction.Create(
			invoiceId: invoice.Id,
			amount: 100,
			paymentMethod: PaymentMethodType.Cash,
			referenceNumber: null,
			currency: Currency.DOP
		);

		invoice.ProcessTransaction(transaction.Value);

		// Act
		var result = invoice.MarkAsPaid();

		// Assert
		result.HasError.Should().BeFalse();
		invoice.Status.Should().Be(InvoiceStatus.Paid);
	}

	[Fact]
	public void MarkAsCancelled_ShouldReturnConflict_WhenInvoiceIsAlreadyCancelled()
	{
		// Arrange
		var invoice = Invoice.Create(customerId: Guid.NewGuid());
		invoice.MarkAsCancelled();

		// Act
		var result = invoice.MarkAsCancelled();

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void MarkAsCancelled_ShouldMarkInvoiceAsCancelled()
	{
		// Arrange
		var invoice = Invoice.Create(customerId: Guid.NewGuid());

		// Act
		var result = invoice.MarkAsCancelled();

		// Assert
		result.HasError.Should().BeFalse();
		invoice.Status.Should().Be(InvoiceStatus.Cancelled);
	}
}
