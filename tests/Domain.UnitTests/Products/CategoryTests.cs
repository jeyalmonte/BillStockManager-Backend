using Domain.Products;
using FluentAssertions;
using SharedKernel.Results;

namespace Domain.UnitTests.Products;
public class CategoryTests
{
	[Fact]
	public void Create_ShouldReturnCategoryInstance()
	{
		// Act
		var category = Category.Create(name: "test1", description: "test1");

		// Assert
		category.Should().NotBeNull();
		category.Should().BeOfType<Category>();
	}

	[Fact]
	public void AddProduct_ShouldReturnConflict_WhenProductAlreadyExists()
	{
		// Arrange
		var category = Category.Create(name: "test1", description: "test1");
		var product = Product.Create(name: "test1", categoryId: category.Id, description: "test1", price: 10, stock: 10);
		product.Should().NotBeNull();
		category.AddProduct(product);

		// Act
		var result = category.AddProduct(product);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void AddProduct_ShouldAddProduct_WhenProductDoesNotExists()
	{
		// Arrange
		var category = Category.Create(name: "test1", description: "test1");
		var product = Product.Create(name: "test1", categoryId: category.Id, description: "test1", price: 10, stock: 10);
		product.Should().NotBeNull();

		// Act
		var result = category.AddProduct(product);

		// Assert
		result.HasError.Should().BeFalse();
		category.Products.Should().Contain(product);
	}

}
