using Application.Customers.Commands.Update;
using Domain.Customers;
using Domain.Customers.Repositories;
using SharedKernel.Interfaces;

namespace Application.UnitTests.Customers.Commands;
public class UpdateCustomerTests
{
    private readonly Mock<ICustomerRepository> _customerRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    [Fact]
    public void UpdateCustomer_WhenFullNameIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var command = Create_UpdateCustomerCommand(includeFullName: false);
        var validator = new UpdateCustomerCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateCustomer_WhenCustomerDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var command = Create_UpdateCustomerCommand();
        _customerRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        var handler = new UpdateCustomerCommandHandler(_customerRepository.Object, _unitOfWork.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.HasError.Should().BeTrue();
        result.Errors.First().ErrorType.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task UpdateCustomer_WhenCustomerExists_ShouldUpdateCustomer()
    {
        // Arrange
        var command = Create_UpdateCustomerCommand();
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

        var handler = new UpdateCustomerCommandHandler(_customerRepository.Object, _unitOfWork.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.HasError.Should().BeFalse();
        result.Value.Should().BeOfType<Success>();
    }

    private static UpdateCustomerCommand Create_UpdateCustomerCommand(bool includeFullName = true)
        => new(
            Guid.NewGuid(),
            includeFullName ? "testUpdate" : string.Empty,
            "test",
            GenderType.Male,
            "test@test.com",
            "1234567890",
            "test");

}
