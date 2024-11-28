using Domain.Inventory;

namespace Infrastructure.Common.Persistence.DataInitializers;
internal static class ProductData
{
	public static List<Product> GetData(List<Category> categories)
	{
		var categoryIds = categories.ToDictionary(c => c.Name, c => c.Id);
		var electronicId = categoryIds["Electronics"];
		var bookId = categoryIds["Books"];
		var beautyId = categoryIds["Beauty"];
		var sportId = categoryIds["Sports"];
		var clothingId = categoryIds["Clothing"];
		var toyId = categoryIds["Toys"];

		return new List<Product>
		{
			Product.Create("Apple iPhone 12 Pro Max", electronicId, "Apple", 1099.99m, 15, 0),
			Product.Create("Samsung Galaxy S21 Ultra", electronicId, "Samsung", 1199.99m, 15, 0),
			Product.Create("Apple MacBook Pro", electronicId, "Apple", 1299.99m, 15, 0),
			Product.Create("Dell XPS 13", electronicId, "Dell", 1199.99m, 15, 0),
			Product.Create("Amazon Kindle Paperwhite", electronicId, "Amazon", 129.99m, 15, 0),
			Product.Create("Apple AirPods Pro", electronicId, "Apple", 249.99m, 15, 0),
			Product.Create("Samsung Galaxy Buds Pro", electronicId, "Samsung", 199.99m, 15, 0),

			Product.Create("The Great Gatsby", bookId, "F. Scott Fitzgerald", 6.99m, 15, 0),
			Product.Create("To Kill a Mockingbird", bookId, "Harper Lee", 7.99m, 15, 0),
			Product.Create("1984", bookId, "George Orwell", 8.99m, 15, 0),
			Product.Create("Animal Farm", bookId, "George Orwell", 6.99m, 15, 0),
			Product.Create("Brave Product.Create World", bookId, "Aldous Huxley", 7.99m, 15, 0),
			Product.Create("Lord of the Flies", bookId, "William Golding", 6.99m, 15, 0),
			Product.Create("The Alchemist", bookId, "Paulo Coelho", 9.99m, 15, 0),

			Product.Create("Dove Beauty Bar", beautyId, "Dove", 4.99m, 15, 0),
			Product.Create("CeraVe Hydrating Facial Cleanser", beautyId, "CeraVe", 14.99m, 15, 0),
			Product.Create("Cetaphil Gentle Skin Cleanser", beautyId, "Cetaphil", 9.99m, 15, 0),
			Product.Create("Olay Regenerist Micro-Sculpting Cream", beautyId, "Olay", 24.99m, 15, 0),
			Product.Create("L'Oréal Paris Revitalift", beautyId, "L'Oréal", 19.99m, 15, 0),
			Product.Create("Neutrogena Hydro Boost", beautyId, "Neutrogena", 16.99m, 15, 0),

			Product.Create("Nike Air Zoom Pegasus 37", sportId, "Nike", 119.99m, 15, 0),
			Product.Create("Adidas Ultraboost 21", sportId, "Adidas", 179.99m, 15, 0),
			Product.Create("Under Armour HOVR Phantom 2", sportId, "Under Armour", 139.99m, 15, 0),
			Product.Create("Product.Create Balance Fresh Foam 1080v11", sportId, "Product.Create Balance", 149.99m, 15, 0),
			Product.Create("Brooks Ghost 13", sportId, "Brooks", 129.99m, 15, 0),
			Product.Create("Asics Gel-Nimbus 23", sportId, "Asics", 149.99m, 15, 0),

			Product.Create("Levi's 501 Original Fit Jeans", clothingId, "Levi's", 49.99m, 15, 0),
			Product.Create("Adidas Originals Trefoil Hoodie", clothingId, "Adidas", 59.99m, 15, 0),
			Product.Create("Champion Reverse Weave Hoodie", clothingId, "Champion", 69.99m, 15, 0),
			Product.Create("Carhartt WIP Michigan Chore Coat", clothingId, "Carhartt", 129.99m, 15, 0),
			Product.Create("The North Face 1996 Retro Nuptse Jacket", clothingId, "The North Face", 249.99m, 15, 0),
			Product.Create("Patagonia Better Sweater", clothingId, "Patagonia", 139.99m, 15, 0),

			Product.Create("LEGO Classic Creative Bricks", toyId, "LEGO", 14.99m, 15, 0),
			Product.Create("Melissa & Doug Wooden Building Blocks Set", toyId, "Melissa & Doug", 19.99m, 15, 0),
			Product.Create("Crayola Inspiration Art Case", toyId, "Crayola", 24.99m, 15, 0),
			Product.Create("Play-Doh Modeling Compound", toyId, "Play-Doh", 9.99m, 15, 0),
			Product.Create("Fisher-Price Laugh & Learn Smart Stages Chair", toyId, "Fisher-Price", 29.99m, 15, 0),
			Product.Create("VTech Sit-to-Stand Learning Walker", toyId, "VTech", 34.99m, 15, 0),
			};
	}
}
