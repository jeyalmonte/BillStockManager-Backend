using Domain.Products;
using FluentAssertions;

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
}
