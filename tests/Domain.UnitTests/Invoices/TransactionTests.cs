using Domain.Invoices;
using FluentAssertions;
using SharedKernel.Results;

namespace Domain.UnitTests.Invoices;
public class TransactionTests
{
	[Fact]
	public void Create_ShouldReturnConflict_WhenAmountIsLessThanOrEqualToZero()
	{
		// Arrange
		var invoiceId = Guid.NewGuid();

		// Act
		var result = Transaction.Create(
			invoiceId: invoiceId,
			amount: 0,
			paymentMethod: PaymentMethodType.Card,
			referenceNumber: "123456",
			currency: Currency.DOP
		);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void Create_ShouldReturnConflict_WhenReferenceNumberIsRequiredAndIsNull()
	{
		// Arrange
		var invoiceId = Guid.NewGuid();

		// Act
		var result = Transaction.Create(
			invoiceId: invoiceId,
			amount: 100,
			paymentMethod: PaymentMethodType.Card,
			referenceNumber: null,
			currency: Currency.DOP
		);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void Create_ShouldReturnTransactionInstance()
	{
		// Arrange
		var invoiceId = Guid.NewGuid();

		// Act
		var result = Transaction.Create(
			invoiceId: invoiceId,
			amount: 100,
			paymentMethod: PaymentMethodType.Card,
			referenceNumber: "123456",
			currency: Currency.DOP
		);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().NotBeNull();
		result.Value.Should().BeOfType<Transaction>();
	}
}
