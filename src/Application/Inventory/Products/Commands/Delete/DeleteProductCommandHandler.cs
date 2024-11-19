using Domain.Inventory.Repositories;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Inventory.Products.Commands.Delete;
public class DeleteProductCommandHandler(
	IProductRepository productRepository,
	IUnitOfWork unitOfWork)
	: ICommandHandler<DeleteProductCommand, Success>
{
	public async Task<Result<Success>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
	{
		var product = await productRepository.GetByIdAsync(
			id: request.Id,
			asNoTracking: false,
			cancellationToken: cancellationToken);

		if (product is null)
		{
			return Error.NotFound(description: "Product not found.");
		}

		product.Remove();
		await unitOfWork.CommitAsync(cancellationToken);

		return Result.Success;
	}
}
