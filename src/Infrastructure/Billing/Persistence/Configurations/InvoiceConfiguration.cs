using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Billing.Persistence.Configurations;
public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
	public void Configure(EntityTypeBuilder<Invoice> builder)
	{
		builder.ToTable("Invoices");

		builder.HasKey(i => i.Id);

		builder.Property(i => i.Id)
			.ValueGeneratedNever();

		builder.Property(i => i.InvoiceNumber)
			.ValueGeneratedOnAdd();

		builder.Property(i => i.TotalAmount)
			.HasPrecision(18, 2);

		builder.Property(i => i.Status)
			.HasConversion(
				c => c.ToString(),
				c => Enum.Parse<InvoiceStatus>(c))
			.IsRequired();

		builder.HasOne(i => i.Customer)
			.WithMany(c => c.Invoices)
			.HasForeignKey(i => i.CustomerId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasMany(i => i.InvoiceDetails)
			.WithOne(i => i.Invoice)
			.HasForeignKey(i => i.InvoiceId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasMany(i => i.Payments)
			.WithOne(i => i.Invoice)
			.HasForeignKey(i => i.InvoiceId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasQueryFilter(i => !i.IsDeleted);
	}
}
