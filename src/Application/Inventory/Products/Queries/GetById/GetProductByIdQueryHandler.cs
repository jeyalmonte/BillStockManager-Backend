using Application.Inventory.Products.Contracts;
using Domain.Inventory.Repositories;
using Mapster;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Inventory.Products.Queries.GetById;
public class GetProductByIdQueryHandler(IProductRepository productRepository)
	: IQueryHandler<GetProductByIdQuery, ProductResponse>
{
	public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
	{
		var product = await productRepository.GetByIdAsync(
			id: request.Id,
			cancellationToken: cancellationToken);

		if (product is null)
		{
			return Error.NotFound(description: $"Product with id {request.Id} not found.");
		}

		return product.Adapt<ProductResponse>();
	}
}
