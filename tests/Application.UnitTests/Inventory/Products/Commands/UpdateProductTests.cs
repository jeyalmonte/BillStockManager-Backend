using Application.Inventory.Products.Commands.Update;
using Domain.Inventory;
using Domain.Inventory.Repositories;
using SharedKernel.Interfaces;

namespace Application.UnitTests.Inventory.Products.Commands;
public class UpdateProductTests
{
	private readonly Mock<IProductRepository> _productRepository = new();
	private readonly Mock<ICategoryRepository> _categoryRepository = new();
	private readonly Mock<IUnitOfWork> _unitOfWork = new();

	[Fact]
	public void UpdateProduct_WhenNameIsEmpty_ShouldReturnValidationError()
	{
		// Arrange
		var command = CreateUpdateProductCommand(hasName: false);

		var validator = new UpdateProductCommandValidator();

		// Act
		var result = validator.Validate(command);

		// Assert
		result.IsValid.Should().BeFalse();
	}

	[Fact]
	public async Task UpdateProduct_WhenProductDoesNotExist_ShouldReturnNotFound()
	{
		// Arrange
		var command = CreateUpdateProductCommand();
		var handler = new UpdateProductCommandHandler(_productRepository.Object, _categoryRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.NotFound);
	}

	[Fact]
	public async Task UpdateProduct_WhenCategoryDoesNotExist_ShouldReturnFailure()
	{
		// Arrange
		var command = CreateUpdateProductCommand();
		var product = Product.Create("test", Guid.NewGuid(), "test", 100, 10, 1);

		_productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(product);

		var handler = new UpdateProductCommandHandler(_productRepository.Object, _categoryRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.Failure);
	}

	[Fact]
	public async Task UpdateProduct_WhenProductExists_ShouldUpdateProduct()
	{
		// Arrange
		var command = CreateUpdateProductCommand();
		var product = Product.Create("test", Guid.NewGuid(), "test", 100, 10, 1);
		var category = Category.Create("test", "test");

		_productRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(product);

		_categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(category);

		var handler = new UpdateProductCommandHandler(_productRepository.Object, _categoryRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().Be(Result.Success);
	}

	private static UpdateProductCommand CreateUpdateProductCommand(bool hasName = true) =>
		new(
			Id: Guid.NewGuid(),
			Name: hasName ? "test" : string.Empty,
			Description: "test",
			CategoryId: Guid.NewGuid(),
			Price: 10,
			Discount: 0);
}
