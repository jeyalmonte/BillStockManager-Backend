using Domain.Billing;
using Domain.Inventory;
using FluentAssertions;
using SharedKernel.Results;

namespace Domain.UnitTests.Billing;
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
	public void AddInvoiceDetail_WhenAddingDuplicateProducts_ShouldReturnConflict()
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
	public void AddInvoiceDetail_WhenAddingUniqueProducts__ShouldAddInvoiceDetail()
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
	public void ProcessPayment_WhenAmountExceedsOutstandingBalance_ShouldReturnConflict()
	{
		// Arrange
		var invoice = Invoice.Create(customerId: Guid.NewGuid());
		var payment = Payment.Create(
			invoiceId: invoice.Id,
			amount: 100,
			paymentMethod: PaymentMethod.Cash,
			referenceNumber: null,
			currency: Currency.DOP
		);

		// Act
		var result = invoice.ProcessPayment(payment.Value);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void ProcessPayment_WhenAmountExceedsTotalAmount_ShouldReturnConflict()
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

		var payment = Payment.Create(
			invoiceId: invoice.Id,
			amount: 200,
			paymentMethod: PaymentMethod.Cash,
			referenceNumber: null,
			currency: Currency.DOP
		);

		// Act
		var result = invoice.ProcessPayment(payment.Value);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void ProcessPayment_ShouldProcessPayment()
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

		var payment = Payment.Create(
			invoiceId: invoice.Id,
			amount: 100,
			paymentMethod: PaymentMethod.Cash,
			referenceNumber: null,
			currency: Currency.DOP
		);

		// Act
		var result = invoice.ProcessPayment(payment.Value);

		// Assert
		result.HasError.Should().BeFalse();
		invoice.Payments.Should().Contain(payment.Value);
	}

	[Fact]
	public void MarkAsPaid_WhenInvoiceIsAlreadyPaid_ShouldReturnConflict()
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

		var transaction = Payment.Create(
			invoiceId: invoice.Id,
			amount: 100,
			paymentMethod: PaymentMethod.Cash,
			referenceNumber: null,
			currency: Currency.DOP
		);

		invoice.ProcessPayment(transaction.Value);
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

		var transaction = Payment.Create(
			invoiceId: invoice.Id,
			amount: 100,
			paymentMethod: PaymentMethod.Cash,
			referenceNumber: null,
			currency: Currency.DOP
		);

		invoice.ProcessPayment(transaction.Value);

		// Act
		var result = invoice.MarkAsPaid();

		// Assert
		result.HasError.Should().BeFalse();
		invoice.Status.Should().Be(InvoiceStatus.Paid);
	}

	[Fact]
	public void MarkAsCancelled_WhenInvoiceIsPaid_ShouldReturnConflict()
	{
		// Arrange
		var invoice = Invoice.Create(customerId: Guid.NewGuid());
		invoice.MarkAsPaid();

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
