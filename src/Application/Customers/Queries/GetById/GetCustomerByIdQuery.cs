using Application.Customers.Contracts;
using SharedKernel.Interfaces.Messaging;

namespace Application.Customers.Queries.GetById;
public record GetCustomerByIdQuery(Guid Id) : IQuery<CustomerResponse>;
