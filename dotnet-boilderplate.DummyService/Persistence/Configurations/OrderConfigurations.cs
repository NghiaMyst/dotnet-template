using dotnet_boilderplate.DummyService.Domains.Aggregates;
using dotnet_boilderplate.DummyService.Domains.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dotnet_boilderplate.DummyService.Persistence.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("order");

            builder.HasKey(o => o.Id);

            builder.Property(b => b.Id)
                .HasConversion(from => Guid.Parse(from.ToString()), reverse => OrderId.From(reverse.ToString()))
                .HasColumnName("id");

            builder.Property(o => o.CustomerId)
                .HasConversion(from => Guid.Parse(from.ToString()), reverse => CustomerId.From(reverse.ToString()))
                .HasColumnName("customer_id")
                .HasMaxLength(50);

            builder.OwnsOne(o => o.TotalAmount, m =>
            {
                m.Property(x => x.Amount).HasColumnName("total_amount").HasPrecision(18, 6);
                m.Property(x => x.Currency).HasColumnName("currency").HasMaxLength(3);
            });

            builder.Property(o => o.Status)
                .HasColumnName("status");

            builder.Property(o => o.Date)
                .HasColumnName("date")
                .HasColumnType("timestamptz");

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
