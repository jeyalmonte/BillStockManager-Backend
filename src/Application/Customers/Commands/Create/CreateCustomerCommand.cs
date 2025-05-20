using Application.Customers.Contracts;
using Domain.Customers;
using SharedKernel.Interfaces.Messaging;

namespace Application.Customers.Commands.Create;
public record CreateCustomerCommand(
    string FullName,
    string? Nickname,
    DocumentType DocumentType,
    string Document,
    GenderType Gender,
    string? Email,
    string? PhoneNumber,
    string? Address) : ICommand<CustomerResponse>;
