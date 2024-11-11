using Domain.Inventory;

namespace Infrastructure.Common.Persistence.DataInitializers;
internal static class CategoryData
{
	public static List<Category> GetData()
		=>
		[
			new("Electronics","Electronic gadgets and accessories"),
			new("Books","Books of all genres"),
			new("Beauty","Beauty and personal care products"),
			new("Sports","Sports and fitness products"),
			new("Clothing","Clothing and accessories"),
			new("Toys","Toys and games"),
		];
}
