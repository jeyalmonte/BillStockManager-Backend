using Application.Billing.Payments.Commands.Make;
using Application.Billing.Payments.Contracts;
using Domain.Billing;
using Domain.Billing.Repositories;
using Domain.Inventory;
using SharedKernel.Interfaces;

namespace Application.UnitTests.Billing.Payments.Commands;
public class MakePaymentTests
{
	private readonly Mock<IInvoiceRepository> _invoiceRepository = new();
	private readonly Mock<IUnitOfWork> _unitOfWork = new();

	[Fact]
	public void MakePayment_WhenAmountIsZero_ShouldReturnValidationError()
	{
		// Arrange
		var command = new MakePaymentCommand(
			InvoiceId: Guid.NewGuid(),
			Amount: 0,
			PaymentMethod: PaymentMethod.Cash,
			ReferenceNumber: default,
			Currency: Currency.USD
		);

		var validator = new MakePaymentCommandValidator();

		// Act
		var result = validator.Validate(command);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().Contain(x => x.PropertyName == nameof(MakePaymentCommand.Amount));
	}

	[Fact]
	public async Task MakePayment_WhenInvoiceDoesNotExist_ShouldReturnNotFound()
	{
		// Arrange
		var command = new MakePaymentCommand(
			InvoiceId: Guid.NewGuid(),
			Amount: 100,
			PaymentMethod: PaymentMethod.Cash,
			ReferenceNumber: default,
			Currency: Currency.USD
		);

		_invoiceRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Invoice?)null);

		var handler = new MakePaymentCommandHandler(_invoiceRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.NotFound);
	}

	[Fact]
	public async Task MakePayment_WhenInvoiceIsAlreadyPaid_ShouldReturnFailure()
	{
		// Arrange
		var command = new MakePaymentCommand(
			InvoiceId: Guid.NewGuid(),
			Amount: 100,
			PaymentMethod: PaymentMethod.Cash,
			ReferenceNumber: default,
			Currency: Currency.USD
		);

		var invoice = Invoice.Create(
			customerId: Guid.NewGuid()
		);

		invoice.MarkAsPaid();

		_invoiceRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(invoice);

		var handler = new MakePaymentCommandHandler(_invoiceRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.Failure);
	}

	[Fact]
	public async Task MakePayment_WhenPaymentFails_ShouldReturnConflict()
	{
		// Arrange
		var command = new MakePaymentCommand(
			InvoiceId: Guid.NewGuid(),
			Amount: 110,
			PaymentMethod: PaymentMethod.Cash,
			ReferenceNumber: default,
			Currency: Currency.USD
		);

		var invoice = Invoice.Create(
			customerId: Guid.NewGuid()
		);

		var product = Product.Create(
			name: "Product 1",
			categoryId: Guid.NewGuid(),
			description: "Product 1 description",
			price: 100,
			stock: 100
			);

		var invoiceDetail = InvoiceDetail.Create(invoice.Id, product, 1);

		invoice.AddInvoiceDetail(invoiceDetail.Value);

		_invoiceRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(invoice);

		var handler = new MakePaymentCommandHandler(_invoiceRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public async Task MakePayment_WhenPaymentSucceeds_ShouldReturnPaymentResponse()
	{
		// Arrange
		var command = new MakePaymentCommand(
			InvoiceId: Guid.NewGuid(),
			Amount: 100,
			PaymentMethod: PaymentMethod.Cash,
			ReferenceNumber: default,
			Currency: Currency.USD
		);
		var invoice = Invoice.Create(
			customerId: Guid.NewGuid()
		);
		var product = Product.Create(
			name: "Product 1",
			categoryId: Guid.NewGuid(),
			description: "Product 1 description",
			price: 100,
			stock: 100
			);

		var invoiceDetail = InvoiceDetail.Create(invoice.Id, product, 1);

		invoice.AddInvoiceDetail(invoiceDetail.Value);

		_invoiceRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(invoice);

		var handler = new MakePaymentCommandHandler(_invoiceRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().BeOfType<PaymentResponse>();
	}
}
