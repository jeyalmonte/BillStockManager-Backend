using Domain.Inventory;
using FluentAssertions;
using SharedKernel.Results;

namespace Domain.UnitTests.Inventory;
public class ProductTests
{
	[Fact]
	public void Create_ShouldReturnProductInstance()
	{
		// Act
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Assert
		product.Should().NotBeNull();
		product.Should().BeOfType<Product>();
	}

	[Fact]
	public void Update_ShouldUpdateProduct()
	{
		// Arrange
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		var nameUpdated = "TestUpdate";

		// Act
		product.Update(
			name: nameUpdated,
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100);

		// Assert
		product.Name.Should().Be(nameUpdated);
	}

	[Fact]
	public void Remove_ShouldMarkProductAsDeleted()
	{
		// Arrange
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		product.Remove();

		// Assert
		product.IsDeleted.Should().BeTrue();
	}

	[Fact]
	public void HandleStockChange_WhenQuantityIsEqualToZero_ShouldReturnFailure()
	{
		// Arrange
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = product.HandleStockChange(quantity: 0);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Failure);
	}

	[Fact]
	public void HandleStockChange_WhenQuantityToIncreaseIsValid_ShouldIncreaseStock()
	{
		// Arrange
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = product.HandleStockChange(quantity: 20);

		// Assert
		result.HasError.Should().BeFalse();
		product.Stock.Should().Be(30);
	}

	[Fact]
	public void HandleStockChange_WhenQuantityToReduceExceedsStock_ShouldReturnConflict()
	{
		// Arrange
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = product.HandleStockChange(quantity: -15);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void HandleStockChange_WhenQuantityToReduceIsValid_ShouldReduceStock()
	{
		// Arrange
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = product.HandleStockChange(quantity: -5);

		// Assert
		result.HasError.Should().BeFalse();
		product.Stock.Should().Be(5);
	}
}
