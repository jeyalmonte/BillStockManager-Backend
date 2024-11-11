using Domain.Inventory;
using FluentAssertions;

namespace Domain.UnitTests.Inventory;
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
