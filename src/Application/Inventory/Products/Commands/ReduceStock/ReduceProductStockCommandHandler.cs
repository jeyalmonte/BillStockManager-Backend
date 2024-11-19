using Domain.Inventory.Repositories;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Inventory.Products.Commands.ReduceStock;
public class ReduceProductStockCommandHandler(
	IProductRepository productRepository,
	IUnitOfWork unitOfWork)
	: ICommandHandler<ReduceProductStockCommand, Success>
{
	public async Task<Result<Success>> Handle(ReduceProductStockCommand request, CancellationToken cancellationToken)
	{
		var product = await productRepository.GetByIdAsync(
			id: request.ProductId,
			asNoTracking: false,
			cancellationToken: cancellationToken);

		if (product is null)
		{
			return Error.NotFound(description: "Product not found.");
		}

		var result = product.ReduceStock(quantity: request.Quantity);

		if (result.HasError)
		{
			return result;
		}

		await unitOfWork.CommitAsync(cancellationToken);

		return Result.Success;
	}
}
