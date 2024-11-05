using Application.Customers.Commands.Create;
using Domain.Customers;
using Domain.Customers.Repositories;
using SharedKernel.Contracts.Customers;
using SharedKernel.Interfaces;

namespace Application.UnitTests.Customers.Commands;
public class CreateCustomerCommandTests
{
	private readonly Mock<ICustomerRepository> _customerRepository = new();
	private readonly Mock<IUnitOfWork> _unitOfWork = new();
	private readonly CancellationToken _cancellationToken = new();

	[Fact]
	public void CreateCustomerCommand_ShouldReturnValidationError_WhenDocumentIsEmpty()
	{
		// Arrange
		var command = Create_CreateCustomerCommand(includeDocument: false);
		var validator = new CreateCustomerCommandValidator();

		// Act
		var result = validator.Validate(command);

		// Assert
		result.IsValid.Should().BeFalse();
	}

	[Fact]
	public async Task CreateCustomerCommand_ShouldReturnError_WhenDocumentExits()
	{
		// Arrange
		var command = Create_CreateCustomerCommand(includeDocument: false);
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

		_customerRepository.Setup(x => x.GetByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(customer);

		var handler = new CreateCustomerCommandHandler(_customerRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, _cancellationToken);

		// Assert
		result.HasError.Should().BeTrue();
	}

	[Fact]
	public async Task CreateCustomerCommand_ShouldCreateCustomer()
	{
		// Arrange
		var command = Create_CreateCustomerCommand();

		_customerRepository.Setup(x => x.GetByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Customer?)null);

		var handler = new CreateCustomerCommandHandler(_customerRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, _cancellationToken);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().NotBeNull();
		result.Value.Should().BeOfType<CustomerResponse>();

		_unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	private static CreateCustomerCommand Create_CreateCustomerCommand(bool includeDocument = true)
		=> new(
			FullName: "test",
			Nickname: "test",
			DocumentType: DocumentType.Cedula,
			Document: includeDocument ? "12345678901" : default!,
			Gender: GenderType.Male,
			Email: "test@test.com",
			PhoneNumber: "1234567890",
			Address: "test");

}
