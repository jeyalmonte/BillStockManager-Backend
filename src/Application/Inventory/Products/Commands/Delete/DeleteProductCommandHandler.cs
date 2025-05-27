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
	private readonly IProductRepository _productRepository = productRepository;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	public async Task<Result<Success>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
	{
		var product = await _productRepository.GetByIdAsync(
			id: request.Id,
			asNoTracking: false,
			cancellationToken: cancellationToken);

		if (product is null)
		{
			return Error.NotFound(description: "Product not found.");
		}

		product.Remove();
		await _unitOfWork.CommitAsync(cancellationToken);

		return Result.Success;
	}
}
