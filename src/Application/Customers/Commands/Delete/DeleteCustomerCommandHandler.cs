using Domain.Customers.Repositories;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Customers.Commands.Delete;
public class DeleteCustomerCommandHandler(
	ICustomerRepository customerRepository,
	IUnitOfWork unitOfWork)
	: ICommandHandler<DeleteCustomerCommand, Success>
{
	public async Task<Result<Success>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
	{
		var client = await customerRepository.GetByIdAsync(
			id: request.Id,
			asNoTracking: false,
			cancellationToken: cancellationToken);

		if (client is null)
		{
			return Error.NotFound(description: "Customer not found.");
		}

		client.MarkAsDeleted();

		await unitOfWork.CommitAsync(cancellationToken);

		return Result.Success;
	}
}
