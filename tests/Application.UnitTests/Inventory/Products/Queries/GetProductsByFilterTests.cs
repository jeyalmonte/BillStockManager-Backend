using Application.Common.Results;
using Application.Inventory.Products.Contracts;
using Application.Inventory.Products.Queries.GetByFilter;
using Domain.Inventory;
using Domain.Inventory.Repositories;
using SharedKernel.Specification;

namespace Application.UnitTests.Inventory.Products.Queries;
public class GetProductsByFilterTests
{
    private readonly Mock<IProductRepository> _productRepository = new();

    [Fact]
    public async Task GetProductsByFilter_ShouldReturnPaginatedResultOfCustomerResponse()
    {
        // Arrange
        var query = new GetProductsByFilterQuery(
            Name: "test",
            SortBy: "test",
            OrderBy: SharedKernel.Enums.OrderType.Descending);

        _productRepository.Setup(x => x.GetAllBySpecAsync(It.IsAny<Specification<Product>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        var handler = new GetProductsByFilterQueryHandler(_productRepository.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.HasError.Should().BeFalse();
        result.Value.Should().BeOfType<PaginatedResult<ProductResponse>>();
    }
}
