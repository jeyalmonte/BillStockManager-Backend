using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Customers.Persistence;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.FullName)
            .IsRequired();

        builder.Property(c => c.Nickname);

        builder.Property(c => c.DocumentType)
            .HasConversion(
            v => v.ToString(),
            v => Enum.Parse<DocumentType>(v))
            .IsRequired();

        builder.Property(c => c.Document)
            .IsRequired();

        builder.Property(c => c.Gender)
           .HasConversion(
               v => v.ToString(),
               v => Enum.Parse<GenderType>(v))
           .IsRequired();

        builder.Property(c => c.Email);

        builder.Property(c => c.PhoneNumber);

        builder.Property(c => c.Address);

        builder
            .HasMany(c => c.Invoices)
            .WithOne(c => c.Customer)
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
