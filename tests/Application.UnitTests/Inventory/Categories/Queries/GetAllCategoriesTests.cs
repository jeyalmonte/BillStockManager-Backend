using Application.Inventory.Categories.Contracts;
using Application.Inventory.Categories.Queries.GetAll;
using Domain.Inventory;
using Domain.Inventory.Repositories;

namespace Application.UnitTests.Inventory.Categories.Queries;
public class GetAllCategoriesTests
{
    private readonly Mock<ICategoryRepository> _categoryRepository = new();

    [Fact]
    public async Task GetAllCategories_ShouldReturnListOfCategoryResponse()
    {
        // Arrange
        _categoryRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Category>());

        var handler = new GetAllCategoriesQueryHandler(_categoryRepository.Object);

        // Act
        var result = await handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

        //Assert
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<List<CategoryResponse>>();
    }
}
