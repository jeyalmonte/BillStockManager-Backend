﻿using Application.Customers.Commands.Delete;
using Domain.Customers;
using Domain.Customers.Repositories;
using SharedKernel.Interfaces;

namespace Application.UnitTests.Customers.Commands;
public class DeleteCustomerTests
{
	private readonly Mock<ICustomerRepository> _customerRepository = new();
	private readonly Mock<IUnitOfWork> _unitOfWork = new();

	[Fact]
	public async Task DeleteCustomer_WhenCustomerDoesNotExist_ShouldReturnNotFound()
	{
		// Arrange
		var command = new DeleteCustomerCommand(Guid.NewGuid());
		_customerRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Customer?)null);

		var handler = new DeleteCustomerCommandHandler(_customerRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().BeOneOf(ErrorType.NotFound);
	}

	[Fact]
	public async Task DeleteCustomer_WhenCustomerExists_ShouldDeleteCustomer()
	{
		// Arrange
		var command = new DeleteCustomerCommand(Guid.NewGuid());
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

		var handler = new DeleteCustomerCommandHandler(_customerRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().Be(Result.Success);
	}
}
