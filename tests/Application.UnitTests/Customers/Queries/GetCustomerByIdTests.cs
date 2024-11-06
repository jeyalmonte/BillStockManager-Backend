using Application.Customers.Queries.GetById;
using Domain.Customers;
using Domain.Customers.Repositories;
using SharedKernel.Contracts.Customers;

namespace Application.UnitTests.Customers.Queries;
public class GetCustomerByIdTests
{
	private readonly Mock<ICustomerRepository> _customerRepository = new();

	[Fact]
	public async Task GetCustomerById_WhenCustomerDoesNotExist_ShouldReturnNotFound()
	{
		// Arrange
		var query = CreateGetCustomerByIdQuery();

		_customerRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Customer?)null);

		var handler = new GetCustomerByIdQueryHandler(_customerRepository.Object);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().Type.Should().Be(ErrorType.NotFound);
	}

	[Fact]
	public async Task GetCustomerById_ShouldReturnCustomerResponse()
	{
		// Arrange
		var query = CreateGetCustomerByIdQuery();

		var customer = Customer.NewBuilder()
			.WithFullName("test")
			.WithNickname("test")
			.WithDocumentType(DocumentType.Cedula)
			.WithDocument("12345678901")
			.WithGender(GenderType.Male)
			.WithEmail("test@test.com")
			.WithPhoneNumber("1234567890")
			.WithAddress("test")
			.Build();

		_customerRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(customer);

		var handler = new GetCustomerByIdQueryHandler(_customerRepository.Object);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().BeOfType<CustomerResponse>();
	}

	private static GetCustomerByIdQuery CreateGetCustomerByIdQuery() => new(Guid.NewGuid());
}
