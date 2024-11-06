using Application.Common.Models;
using Application.Customers.Queries.GetByFilter;
using Domain.Customers;
using Domain.Customers.Repositories;
using SharedKernel.Contracts.Customers;
using SharedKernel.Specification;

namespace Application.UnitTests.Customers.Queries;
public class GetCustomerByFilterTests
{
	private readonly Mock<ICustomerRepository> _customerRepository = new();

	[Fact]
	public async Task GetCustomerByFilter_ShouldReturnPaginatedResult()
	{
		// Arrange
		var query = new GetCustomersByFilterQuery(
			FullName: "test",
			Nickname: "test",
			DocumentType: DocumentType.Cedula,
			Document: "12345678901",
			Gender: GenderType.Male,
			SortBy: "FullName",
			OrderBy: SharedKernel.Enums.OrderType.Descending);

		_customerRepository.Setup(x => x.GetAllBySpecAsync(It.IsAny<Specification<Customer>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<Customer>());

		var handler = new GetCustomersByFilterQueryHandler(_customerRepository.Object);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		result.Should().BeOfType<PaginatedResult<CustomerResponse>>();
	}
}
