using Application.Billing.Invoices.Queries.GetById;
using Domain.Billing;
using Domain.Billing.Repositories;
using SharedKernel.Contracts.Invoices;

namespace Application.UnitTests.Billing.Invoices;
public class GetInvoiceByIdTests
{
	private readonly Mock<IInvoiceRepository> _invoiceRepository = new();

	[Fact]
	public async Task GetInvoiceById_WhenInvoiceNotFound_ShouldReturnNotFound()
	{
		// Arrange
		var query = new GetInvoiceByIdQuery(Guid.NewGuid());

		_invoiceRepository.Setup(x => x.GetByIdWithCustomerAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Invoice?)null);

		var handler = new GetInvoiceByIdQueryHandler(_invoiceRepository.Object);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.NotFound);
	}

	[Fact]
	public async Task GetInvoiceById_WhenInvoiceExists_ShouldReturnInvoiceResponse()
	{
		// Arrange
		var query = new GetInvoiceByIdQuery(Guid.NewGuid());
		var invoice = Invoice.Create(Guid.NewGuid());

		_invoiceRepository.Setup(x => x.GetByIdWithCustomerAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(invoice);

		var handler = new GetInvoiceByIdQueryHandler(_invoiceRepository.Object);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().BeOfType<InvoiceResponse>();
	}

}
