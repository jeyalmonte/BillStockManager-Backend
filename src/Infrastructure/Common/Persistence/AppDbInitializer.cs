using Domain.Customers;
using Domain.Products;
using Infrastructure.Common.Persistence.DataInitializers;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Persistence;

public interface IAppDbInitializer
{
    Task InitializeAsync();
    Task SeedAsync();
}

internal class AppDbInitializer(
    AppDbContext db,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IAppDbInitializer
{
    private const string AdministratorRoleName = "Administrator";

    public async Task InitializeAsync()
    {
        await db.Database.MigrateAsync();
    }

    public async Task SeedAsync()
    {
        await SeedAdministratorAsync();

        var categories = CategoryData.GetData();

        if (!db.Set<Category>().Any())
        {
            db.AddRange(categories);
        }
        if (!db.Set<Product>().Any())
            db.AddRange(ProductData.GetData(categories));
        if (!db.Set<Customer>().Any())
            db.AddRange(CustomerData.GetData());

        await db.SaveChangesAsync(CancellationToken.None);
    }

    private async Task SeedAdministratorAsync()
    {
        if (await roleManager.RoleExistsAsync(AdministratorRoleName))
            return;

        var adminRole = new IdentityRole(AdministratorRoleName);
        await roleManager.CreateAsync(adminRole);

        var adminUser = new User("Admin", "admin", "admin@admin.com");
        await userManager.CreateAsync(adminUser, "admin1234");
        await userManager.AddToRoleAsync(adminUser, AdministratorRoleName);
    }
}
