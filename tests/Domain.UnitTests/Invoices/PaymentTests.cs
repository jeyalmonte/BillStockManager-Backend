using Domain.Invoices;
using FluentAssertions;
using SharedKernel.Results;

namespace Domain.UnitTests.Invoices;
public class PaymentTests
{
	[Fact]
	public void Create_WhenAmountIsLessThanOrEqualToZero_ShouldReturnConflict()
	{
		// Arrange
		var invoiceId = Guid.NewGuid();

		// Act
		var result = Payment.Create(
			invoiceId: invoiceId,
			amount: 0,
			paymentMethod: PaymentMethod.Card,
			referenceNumber: "123456",
			currency: Currency.DOP
		);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void Create_WhenReferenceNumberIsNull_ShouldReturnConflict()
	{
		// Arrange
		var invoiceId = Guid.NewGuid();

		// Act
		var result = Payment.Create(
			invoiceId: invoiceId,
			amount: 100,
			paymentMethod: PaymentMethod.Card,
			referenceNumber: null,
			currency: Currency.DOP
		);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void Create_ShouldReturnPaymentInstance()
	{
		// Arrange
		var invoiceId = Guid.NewGuid();

		// Act
		var result = Payment.Create(
			invoiceId: invoiceId,
			amount: 100,
			paymentMethod: PaymentMethod.Card,
			referenceNumber: "123456",
			currency: Currency.DOP
		);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().NotBeNull();
		result.Value.Should().BeOfType<Payment>();
	}
}
