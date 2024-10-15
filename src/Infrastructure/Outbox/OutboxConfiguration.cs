using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Outbox;

public class OutboxConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
	public void Configure(EntityTypeBuilder<OutboxMessage> builder)
	{
		//NOSONAR
		//builder.ToTable("OutboxMessages");

		builder.HasKey(e => e.Id);

		builder.Property(e => e.Id)
			.ValueGeneratedNever();

		builder.Property(e => e.Type);

		builder.Property(e => e.Content);

		builder.Property(e => e.CreatedOn);

		builder.Property(e => e.Processed);

		builder.Property(e => e.ProcessedOn);
	}
}
