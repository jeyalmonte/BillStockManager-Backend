using Domain.Inventory;

namespace Infrastructure.Common.Persistence.DataInitializers;
internal static class CategoryData
{
	public static List<Category> GetData()
		=>
		[
			Category.Create("Electronics","Electronic gadgets and accessories"),
			Category.Create("Books","Books of all genres"),
			Category.Create("Beauty","Beauty and personal care products"),
			Category.Create("Sports","Sports and fitness products"),
			Category.Create("Clothing","Clothing and accessories"),
			Category.Create("Toys","Toys and games"),
		];
}
