using Application.Inventory.Products.Commands.Create;
using Domain.Inventory;
using Domain.Inventory.Repositories;
using SharedKernel.Contracts.Products;
using SharedKernel.Interfaces;

namespace Application.UnitTests.Inventory.Products.Commands;
public class CreateProductTests
{
    private readonly Mock<IProductRepository> _productRepository = new();
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    [Fact]
    public void CreateProduct_WhenNameIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var command = Create_CreateProductCommand(hasName: false);
        var validator = new CreateProductCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateProduct_WhenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var command = Create_CreateProductCommand();
        var handler = new CreateProductCommandHandler(_productRepository.Object, _categoryRepository.Object, _unitOfWork.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.HasError.Should().BeTrue();
        result.Errors.Single().ErrorType.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task CreateProduct_WhenExistProductWithSameName_ShouldReturnConflict()
    {
        // Arrange
        var command = Create_CreateProductCommand();
        var category = Category.Create("test", "test");
        var product = Product.Create("test", Guid.NewGuid(), "test", 100, 10, 1);

        _categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _productRepository.Setup(x => x.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var handler = new CreateProductCommandHandler(_productRepository.Object, _categoryRepository.Object, _unitOfWork.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.HasError.Should().BeTrue();
        result.Errors.Single().ErrorType.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task CreateProduct_ShouldCreateProduct()
    {
        // Arrange
        var command = Create_CreateProductCommand();
        var category = Category.Create("test", "test");

        _categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var handler = new CreateProductCommandHandler(_productRepository.Object, _categoryRepository.Object, _unitOfWork.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.HasError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<ProductResponse>();

        _productRepository.Verify(x => x.Add(It.IsAny<Product>()), Times.Once());
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    private static CreateProductCommand Create_CreateProductCommand(bool hasName = true)
        => new(
            Name: hasName ? "test" : default!,
            CategoryId: Guid.NewGuid(),
            Description: "test",
            Price: 1000,
            Stock: 50,
            Discount: 5);
}
