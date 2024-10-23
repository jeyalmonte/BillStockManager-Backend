using Domain.Invoices;
using Domain.Products;
using FluentAssertions;
using SharedKernel.Results;

namespace Domain.UnitTests.Invoices;
public class InvoiceDetailTests
{
	[Fact]
	public void Create_ShouldReturnConflict_WhenQuantityIsLessThanOrEqualToZero()
	{
		// Arrange
		var invoiceId = Guid.NewGuid();
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = InvoiceDetail.Create(
			invoiceId: invoiceId,
			product: product,
			quantity: 0
		);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void Create_ShouldReturnConflict_WhenUnitPriceIsLessThanOrEqualToZero()
	{
		// Arrange
		var invoiceId = Guid.NewGuid();
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 0,
			stock: 10
		);

		// Act
		var result = InvoiceDetail.Create(
			invoiceId: invoiceId,
			product: product,
			quantity: 1
		);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void Create_ShouldReturnConflict_WhenDiscountIsNegative()
	{
		// Arrange
		var invoiceId = Guid.NewGuid();
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = InvoiceDetail.Create(
			invoiceId: invoiceId,
			product: product,
			quantity: 1,
			discount: -1
		);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void Create_ShouldReturnAnInvoiceDetailInstance()
	{
		// Arrange
		var invoiceId = Guid.NewGuid();
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = InvoiceDetail.Create(
			invoiceId: invoiceId,
			product: product,
			quantity: 1
		);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().NotBeNull();
		result.Value.Should().BeOfType<InvoiceDetail>();
	}
}
