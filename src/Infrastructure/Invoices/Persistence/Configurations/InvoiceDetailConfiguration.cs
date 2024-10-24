using Domain.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Invoices.Persistence.Configurations;
public class InvoiceDetailConfiguration : IEntityTypeConfiguration<InvoiceDetail>
{
	public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
	{
		builder.ToTable("InvoiceDetails");

		builder.HasKey(i => i.Id);

		builder.Property(i => i.Id)
			.ValueGeneratedNever();

		builder.Property(i => i.Quantity);

		builder.Property(i => i.UnitPrice)
			.HasPrecision(18, 2);

		builder.Property(i => i.Discount)
			.HasPrecision(18, 2);

		builder.HasOne(i => i.Invoice)
			.WithMany(i => i.InvoiceDetails)
			.HasForeignKey(i => i.InvoiceId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(i => i.Product)
			.WithMany()
			.HasForeignKey(i => i.ProductId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasQueryFilter(i => !i.IsDeleted);
	}
}
