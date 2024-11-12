using Application.Inventory.Categories.Commands.Create;
using Domain.Inventory;
using Domain.Inventory.Repositories;
using SharedKernel.Contracts.Categories;
using SharedKernel.Interfaces;

namespace Application.UnitTests.Inventory.Categories.Commands;
public class CreateCategoryTests
{
	private readonly Mock<ICategoryRepository> _categoryRepository = new();
	private readonly Mock<IUnitOfWork> _unitOfWork = new();

	[Fact]
	public void CreateCategory_WhenCategoryNameIsEmpty_ShouldReturnValidationError()
	{
		// Arrange
		var command = Create_CreateCategoryCommand(hasName: false);
		var validator = new CreateCategoryCommandValidator();

		// Act
		var result = validator.Validate(command);

		// Assert
		result.IsValid.Should().BeFalse();
	}

	[Fact]
	public async Task CreateCategory_WhenCategoryExists_ShouldReturnConflictError()
	{
		// Arrange
		var command = Create_CreateCategoryCommand();
		var validator = new CreateCategoryCommandValidator();
		var category = Category.Create("test", "test");

		_categoryRepository.Setup(x => x.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(category);

		var handler = new CreateCategoryCommandHandler(_categoryRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().BeOneOf(ErrorType.Conflict);
	}

	[Fact]
	public async Task CreateCategory_ShouldCreateCategory()
	{
		// Arrange
		var command = Create_CreateCategoryCommand();
		var validator = new CreateCategoryCommandValidator();

		_categoryRepository.Setup(x => x.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Category?)null);


		var handler = new CreateCategoryCommandHandler(_categoryRepository.Object, _unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().BeOfType<CategoryResponse>();

		_categoryRepository.Verify(x => x.Add(It.IsAny<Category>()), Times.Once);
		_unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

	}

	private static CreateCategoryCommand Create_CreateCategoryCommand(bool hasName = true)
		=> new(
			Name: hasName ? "test" : default!,
			Description: "test"
			);
}
