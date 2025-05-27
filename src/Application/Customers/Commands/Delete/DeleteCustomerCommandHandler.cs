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
	private readonly ICustomerRepository _customerRepository = customerRepository;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task<Result<Success>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
	{
		var client = await _customerRepository.GetByIdAsync(
			id: request.Id,
			asNoTracking: false,
			cancellationToken: cancellationToken);

		if (client is null)
		{
			return Error.NotFound(description: "Customer not found.");
		}

		client.MarkAsDeleted();

		await _unitOfWork.CommitAsync(cancellationToken);

		return Result.Success;
	}
}
