using Application.Customers.Contracts;
using Domain.Customers;
using Domain.Customers.Repositories;
using Mapster;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Customers.Commands.Create;
public class CreateCustomerCommandHandler(
	ICustomerRepository customerRepository,
	IUnitOfWork unitOfWork)
	: ICommandHandler<CreateCustomerCommand, CustomerResponse>
{
	private readonly ICustomerRepository _customerRepository = customerRepository;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task<Result<CustomerResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
	{
		var existingDocument = await _customerRepository.GetByDocumentAsync(request.Document, cancellationToken);

		if (existingDocument is not null)
		{
			return Error.Conflict(description: $"The document '{request.Document}' already exists.");
		}

		var newCustomer = Customer.NewBuilder()
		   .WithFullName(request.FullName)
		   .WithNickname(request.Nickname)
		   .WithDocumentType(request.DocumentType)
		   .WithDocument(request.Document)
		   .WithGender(request.Gender)
		   .WithEmail(request.Email)
		   .WithPhoneNumber(request.PhoneNumber)
		   .WithAddress(request.Address)
		   .Build();

		_customerRepository.Add(newCustomer);
		await _unitOfWork.CommitAsync(cancellationToken);

		return newCustomer.Adapt<CustomerResponse>();
	}
}
