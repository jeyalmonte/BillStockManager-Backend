using Domain.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Invoices.Persistence.Configurations;
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
	public void Configure(EntityTypeBuilder<Payment> builder)
	{
		builder.ToTable("Payments");

		builder.HasKey(t => t.Id);

		builder.Property(t => t.Id)
			.ValueGeneratedNever();

		builder.Property(t => t.Amount)
			.HasPrecision(18, 2);

		builder.Property(t => t.Currency)
			.HasConversion(
				c => c.ToString(),
				c => Enum.Parse<Currency>(c))
			.IsRequired();

		builder.Property(t => t.PaymentMethod)
			.HasConversion(
				p => p.ToString(),
				p => Enum.Parse<PaymentMethod>(p))
			.IsRequired();

		builder.Property(t => t.ReferenceNumber);

		builder.HasOne(t => t.Invoice)
			.WithMany(i => i.Payments)
			.HasForeignKey(t => t.InvoiceId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasQueryFilter(t => !t.IsDeleted);
	}
}
