using dotnet_boilderplate.DummyService.Domains.Entities;
using dotnet_boilderplate.DummyService.Domains.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dotnet_boilderplate.DummyService.Persistence.Configurations
{
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("outbox_message");

            builder.HasKey(o => o.Id);

            builder.Property(b => b.Id)
                .HasConversion(from => Guid.Parse(from.ToString()), reverse => OutboxMessageId.From(reverse.ToString()))
                .HasColumnName("id");

            builder.Property(b => b.Type)
                .HasColumnName("type");

            builder.Property(b => b.Published)
                .HasColumnName("published");

            builder.Property(b => b.Payload)
                .HasColumnName("payload");

            // Audit Properties
            builder.Property(o => o.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz");
            builder.Property(o => o.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamptz")
                .IsRequired(false);
            builder.Property(o => o.CreatedBy)
                .HasColumnName("created_by")
                .HasMaxLength(100);
            builder.Property(o => o.UpdatedBy)
                .HasColumnName("updated_by")
                .HasMaxLength(100)
                .IsRequired(false);
        }
    }
}
