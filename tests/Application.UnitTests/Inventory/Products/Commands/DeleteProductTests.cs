using Application.Inventory.Products.Commands.Delete;
using Domain.Inventory;
using Domain.Inventory.Repositories;
using SharedKernel.Interfaces;

namespace Application.UnitTests.Inventory.Products.Commands;
public class DeleteProductTests
{
	private readonly Mock<IProductRepository> _productRepository = new();
	private readonly Mock<IUnitOfWork> _unitOfWork = new();

	[Fact]
	public async Task DeleteProduct_WhenProductDoesNotExist_ShouldReturnNotFound()
	{
		// Arrange
		var command = new DeleteProductCommand(Guid.NewGuid());

		_productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Product?)null);

		var handler = new DeleteProductCommandHandler(_productRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.NotFound);
	}

	[Fact]
	public async Task DeleteProduct_WhenProductExists_ShouldDeleteProduct()
	{
		// Arrange
		var product = Product.Create("test", Guid.NewGuid(), "test", 10, 10, 10);
		var command = new DeleteProductCommand(product.Id);

		_productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(product);

		var handler = new DeleteProductCommandHandler(_productRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeFalse();
		product.IsDeleted.Should().BeTrue();
		_unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}
