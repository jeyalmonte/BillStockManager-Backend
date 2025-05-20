using Application.Billing.Invoices.Contracts;
using Application.Billing.Invoices.Queries.GetByFilter;
using Application.Common.Results;
using Domain.Billing;
using Domain.Billing.Repositories;
using SharedKernel.Specification;

namespace Application.UnitTests.Billing.Invoices.Queries;
public class GetInvoicesByFilterTests
{
	private readonly Mock<IInvoiceRepository> _invoiceRepository = new();

	[Fact]
	public async Task GetInvoicesByFilter_ShouldReturnPaginatedResult()
	{
		// Arrange
		var query = new GetInvoicesByFilterQuery(
			InvoiceNumber: null,
			CustomerName: null,
			Status: null,
			SortBy: null);

		var invoices = new List<Invoice>
		{
			Invoice.Create(Guid.NewGuid()),
			Invoice.Create(Guid.NewGuid())
		};

		_invoiceRepository.Setup(x => x.GetTotalAsync(It.IsAny<Specification<Invoice>>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(invoices.Count);

		_invoiceRepository.Setup(x => x.GetAllBySpecAsync(It.IsAny<Specification<Invoice>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(invoices);

		var handler = new GetInvoicesByFilterQueryHandler(_invoiceRepository.Object);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Items.Should().HaveCount(2);
		result.Value.Should().BeOfType<PaginatedResult<InvoiceResponse>>();

		_invoiceRepository.Verify(x => x.GetTotalAsync(It.IsAny<Specification<Invoice>>(), It.IsAny<CancellationToken>()), Times.Once);
		_invoiceRepository.Verify(x => x.GetAllBySpecAsync(It.IsAny<Specification<Invoice>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
	}

}
