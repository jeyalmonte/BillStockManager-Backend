using Domain.Customers;
using SharedKernel.Contracts.Customers;
using SharedKernel.Interfaces.Messaging;

namespace Application.Customers.Create;
public record CreateCustomerCommand(
    string FullName,
    string? Nickname,
    DocumentType DocumentType,
    string Document,
    GenderType Gender,
    string? Email,
    string? PhoneNumber,
    string? Address) : ICommand<CustomerResponse>;
