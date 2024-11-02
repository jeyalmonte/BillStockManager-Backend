using Domain.Products;

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
			new("Apple iPhone 12 Pro Max", electronicId, "Apple", 1099.99m, 15, 0),
			new("Samsung Galaxy S21 Ultra", electronicId, "Samsung", 1199.99m, 15, 0),
			new("Apple MacBook Pro", electronicId, "Apple", 1299.99m, 15, 0),
			new("Dell XPS 13", electronicId, "Dell", 1199.99m, 15, 0),
			new("Amazon Kindle Paperwhite", electronicId, "Amazon", 129.99m, 15, 0),
			new("Apple AirPods Pro", electronicId, "Apple", 249.99m, 15, 0),
			new("Samsung Galaxy Buds Pro", electronicId, "Samsung", 199.99m, 15, 0),

			new("The Great Gatsby", bookId, "F. Scott Fitzgerald", 6.99m, 15, 0),
			new("To Kill a Mockingbird", bookId, "Harper Lee", 7.99m, 15, 0),
			new("1984", bookId, "George Orwell", 8.99m, 15, 0),
			new("Animal Farm", bookId, "George Orwell", 6.99m, 15, 0),
			new("Brave New World", bookId, "Aldous Huxley", 7.99m, 15, 0),
			new("Lord of the Flies", bookId, "William Golding", 6.99m, 15, 0),
			new("The Alchemist", bookId, "Paulo Coelho", 9.99m, 15, 0),

			new("Dove Beauty Bar", beautyId, "Dove", 4.99m, 15, 0),
			new("CeraVe Hydrating Facial Cleanser", beautyId, "CeraVe", 14.99m, 15, 0),
			new("Cetaphil Gentle Skin Cleanser", beautyId, "Cetaphil", 9.99m, 15, 0),
			new("Olay Regenerist Micro-Sculpting Cream", beautyId, "Olay", 24.99m, 15, 0),
			new("L'Oréal Paris Revitalift", beautyId, "L'Oréal", 19.99m, 15, 0),
			new("Neutrogena Hydro Boost", beautyId, "Neutrogena", 16.99m, 15, 0),

			new("Nike Air Zoom Pegasus 37", sportId, "Nike", 119.99m, 15, 0),
			new("Adidas Ultraboost 21", sportId, "Adidas", 179.99m, 15, 0),
			new("Under Armour HOVR Phantom 2", sportId, "Under Armour", 139.99m, 15, 0),
			new("New Balance Fresh Foam 1080v11", sportId, "New Balance", 149.99m, 15, 0),
			new("Brooks Ghost 13", sportId, "Brooks", 129.99m, 15, 0),
			new("Asics Gel-Nimbus 23", sportId, "Asics", 149.99m, 15, 0),

			new("Levi's 501 Original Fit Jeans", clothingId, "Levi's", 49.99m, 15, 0),
			new("Adidas Originals Trefoil Hoodie", clothingId, "Adidas", 59.99m, 15, 0),
			new("Champion Reverse Weave Hoodie", clothingId, "Champion", 69.99m, 15, 0),
			new("Carhartt WIP Michigan Chore Coat", clothingId, "Carhartt", 129.99m, 15, 0),
			new("The North Face 1996 Retro Nuptse Jacket", clothingId, "The North Face", 249.99m, 15, 0),
			new("Patagonia Better Sweater", clothingId, "Patagonia", 139.99m, 15, 0),

			new("LEGO Classic Creative Bricks", toyId, "LEGO", 14.99m, 15, 0),
			new("Melissa & Doug Wooden Building Blocks Set", toyId, "Melissa & Doug", 19.99m, 15, 0),
			new("Crayola Inspiration Art Case", toyId, "Crayola", 24.99m, 15, 0),
			new("Play-Doh Modeling Compound", toyId, "Play-Doh", 9.99m, 15, 0),
			new("Fisher-Price Laugh & Learn Smart Stages Chair", toyId, "Fisher-Price", 29.99m, 15, 0),
			new("VTech Sit-to-Stand Learning Walker", toyId, "VTech", 34.99m, 15, 0),
			};
	}
}
