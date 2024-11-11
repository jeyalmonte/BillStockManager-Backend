using Domain.Customers;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Customers.Commands.Update;
public record UpdateCustomerCommand(
	Guid Id,
	string FullName,
	string? Nickname,
	GenderType Gender,
	string? Email,
	string? PhoneNumber,
	string? Address) : ICommand<Success>;

