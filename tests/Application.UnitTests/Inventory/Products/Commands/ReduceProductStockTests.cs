using Application.Inventory.Products.Commands.ReduceStock;
using Domain.Inventory;
using Domain.Inventory.Repositories;
using SharedKernel.Interfaces;

namespace Application.UnitTests.Inventory.Products.Commands;
public class ReduceProductStockTests
{
	private readonly Mock<IProductRepository> _productRepository = new();
	private readonly Mock<IUnitOfWork> _unitOfWork = new();

	[Fact]
	public async Task ReduceProductStock_WhenProductNotFound_ShouldReturnNotFoundError()
	{
		// Arrange
		_productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Product?)null);

		var handler = new ReduceProductStockCommandHandler(
			productRepository: _productRepository.Object,
			unitOfWork: _unitOfWork.Object);

		var command = CreateReduceProductStockCommand();

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.NotFound);
	}

	[Fact]
	public async Task ReduceProductStock_WhenQuantityIsLessThanZero_ShouldReturnFailureError()
	{
		// Arrange
		var product = Product.Create(
			name: "Product",
			categoryId: Guid.NewGuid(),
			description: "Description",
			price: 10,
			stock: 10);

		_productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(product);

		var handler = new ReduceProductStockCommandHandler(
			productRepository: _productRepository.Object,
			unitOfWork: _unitOfWork.Object);

		var command = CreateReduceProductStockCommand(quantity: -1);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.Failure);
	}

	[Fact]
	public async Task ReduceProductStock_WhenQuantityExceedsStock_ShouldReturnConflictError()
	{
		// Arrange
		var product = Product.Create(
			name: "Product",
			categoryId: Guid.NewGuid(),
			description: "Description",
			price: 10,
			stock: 1);

		_productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(product);

		var handler = new ReduceProductStockCommandHandler(
			productRepository: _productRepository.Object,
			unitOfWork: _unitOfWork.Object);

		var command = CreateReduceProductStockCommand(quantity: 2);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public async Task ReduceProductStock_WhenProductExistsAndQuantityIsValid_ShouldReduceStock()
	{
		// Arrange
		var product = Product.Create(
			name: "Product",
			categoryId: Guid.NewGuid(),
			description: "Description",
			price: 10,
			stock: 10);

		_productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(product);

		var handler = new ReduceProductStockCommandHandler(
			productRepository: _productRepository.Object,
			unitOfWork: _unitOfWork.Object);

		var command = CreateReduceProductStockCommand();

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeFalse();
		product.Stock.Should().Be(9);
	}

	private static ReduceProductStockCommand CreateReduceProductStockCommand(int quantity = 1)
	{
		return new ReduceProductStockCommand(
			ProductId: Guid.NewGuid(),
			Quantity: quantity);
	}

}
