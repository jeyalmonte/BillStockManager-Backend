using Application.Inventory.Products.Contracts;
using Application.Inventory.Products.Queries.GetById;
using Domain.Inventory;
using Domain.Inventory.Repositories;

namespace Application.UnitTests.Inventory.Products.Queries;
public class GetProductByIdTests
{
	private readonly Mock<IProductRepository> _productRepository = new();

	[Fact]
	public async Task GetProductById_WhenProductDoesNotExist_ShouldReturnNotFound()
	{
		// Arrange
		var query = new GetProductByIdQuery(Guid.NewGuid());

		_productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Product?)null);

		var handler = new GetProductByIdQueryHandler(_productRepository.Object);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.NotFound);
	}

	[Fact]
	public async Task GetProductById_ShouldReturnProductResponse()
	{
		// Arrange
		var query = new GetProductByIdQuery(Guid.NewGuid());

		var product = Product.Create("test", Guid.NewGuid(), "test", 100, 100, 0);
		_productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(product);

		var handler = new GetProductByIdQueryHandler(_productRepository.Object);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().BeOfType<ProductResponse>();
	}

}
