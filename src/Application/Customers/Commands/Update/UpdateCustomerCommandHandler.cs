using Domain.Customers.Repositories;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Customers.Commands.Update;
public class UpdateCustomerCommandHandler(
	ICustomerRepository customerRepository,
	IUnitOfWork unitOfWork)
	: ICommandHandler<UpdateCustomerCommand, Success>
{
	private readonly ICustomerRepository _customerRepository = customerRepository;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task<Result<Success>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
	{
		var customer = await _customerRepository.GetByIdAsync(
			id: request.Id,
			asNoTracking: false,
			cancellationToken: cancellationToken);

		if (customer is null)
		{
			return Error.NotFound(description: "Customer not found.");
		}

		customer.Update(
			fullName: request.FullName,
			nickname: request.Nickname,
			email: request.Email,
			phoneNumber: request.PhoneNumber,
			address: request.Address
			);

		await _unitOfWork.CommitAsync(cancellationToken);

		return Result.Success;
	}
}
