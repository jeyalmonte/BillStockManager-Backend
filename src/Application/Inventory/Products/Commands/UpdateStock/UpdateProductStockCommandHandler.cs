using Domain.Inventory.Repositories;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Inventory.Products.Commands.UpdateStock;
public class UpdateProductStockCommandHandler(
	IProductRepository productRepository,
	IUnitOfWork unitOfWork)
	: ICommandHandler<UpdateProductStockCommand, Success>
{
	public async Task<Result<Success>> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
	{
		var product = await productRepository.GetByIdAsync(
			id: request.ProductId,
			asNoTracking: false,
			cancellationToken: cancellationToken);

		if (product is null)
		{
			return Error.NotFound(description: "Product not found.");
		}

		var result = product.HandleStockChange(request.Quantity);

		if (result.HasError)
		{
			return result.Errors;
		}

		await unitOfWork.CommitAsync(cancellationToken);

		return Result.Success;
	}
}
