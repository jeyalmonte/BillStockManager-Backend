using Application.Billing.Invoices.Commands.Cancel;
using Domain.Billing;
using Domain.Billing.Repositories;
using SharedKernel.Interfaces;

namespace Application.UnitTests.Billing.Invoices.Commands;
public class CancelInvoiceTests
{
	private readonly Mock<IInvoiceRepository> _invoiceRepository = new();
	private readonly Mock<IUnitOfWork> _unitOfWork = new();

	[Fact]
	public async Task CancelInvoice_WhenInvoiceDoesNotExist_ShouldReturnNotFound()
	{
		// Arrange
		var command = new CancelInvoiceCommand(Guid.NewGuid());

		_invoiceRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Invoice?)null);

		var handler = new CancelInvoiceCommandHandler(_invoiceRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.NotFound);
	}

	[Fact]
	public async Task CancelInvoice_MarkAsCancelledFails_ShouldReturnConflictError()
	{
		// Arrange
		var invoice = Invoice.Create(Guid.NewGuid());
		invoice.MarkAsPaid();

		var command = new CancelInvoiceCommand(invoice.Id);

		_invoiceRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(invoice);

		var handler = new CancelInvoiceCommandHandler(_invoiceRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public async Task CancelInvoice_WhenCommandIsValid_ShouldCancelInvoice()
	{
		// Arrange
		var invoice = Invoice.Create(Guid.NewGuid());
		var command = new CancelInvoiceCommand(invoice.Id);

		_invoiceRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(invoice);

		var handler = new CancelInvoiceCommandHandler(_invoiceRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeFalse();
		invoice.Status.Should().Be(InvoiceStatus.Cancelled);

		_unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}
