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
	public async Task<Result<Success>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
	{
		var customer = await customerRepository.GetByIdAsync(
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

		await unitOfWork.CommitAsync(cancellationToken);

		return Result.Success;
	}
}
