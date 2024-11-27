using Application.Billing.Invoices.Queries.GetDetails;
using Domain.Billing;
using Domain.Billing.Repositories;
using SharedKernel.Contracts.Invoices;

namespace Application.UnitTests.Billing.Invoices;
public class GetInvoiceDetailsTests
{
	private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock = new();
	private readonly Mock<IInvoiceDetailRepository> _invoiceDetailRepositoryMock = new();

	[Fact]
	public async Task GetInvoiceDetails_WhenInvoiceDoesNotExist_ReturnNotFound()
	{
		// Arrange
		var query = new GetInvoiceDetailsQuery(Guid.NewGuid());

		_invoiceRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Invoice?)null);

		var handler = new GetInvoiceDetailsQueryHandler(_invoiceRepositoryMock.Object, _invoiceDetailRepositoryMock.Object);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.NotFound);
	}

	[Fact]
	public async Task GetInvoiceDetails_WhenInvoiceExists_ReturnInvoiceDetails()
	{
		// Arrange
		var query = new GetInvoiceDetailsQuery(Guid.NewGuid());
		var invoice = Invoice.Create(Guid.NewGuid());
		var details = new List<InvoiceDetail>();

		_invoiceRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(invoice);

		_invoiceDetailRepositoryMock.Setup(x => x.GetDetailsByInvoiceId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(details);

		var handler = new GetInvoiceDetailsQueryHandler(_invoiceRepositoryMock.Object, _invoiceDetailRepositoryMock.Object);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().BeOfType<List<InvoiceDetailResponse>>();
	}
}
