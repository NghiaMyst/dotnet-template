using dotnet_boilderplate.DummyService.Domains.Entities;
using dotnet_boilderplate.SharedKernel.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dotnet_boilderplate.DummyService.Persistence.Configurations
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_item");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .HasColumnName("id")
                .HasConversion(id => id.ToString(), str => EntityId.From(str))
                .HasMaxLength(50);

            builder.Property(o => o.OrderId)
                .HasColumnName("order_id")
                .HasConversion(from => Guid.Parse(from), reverse => reverse.ToString());

            builder.Property(o => o.ProductName)
                .HasColumnName("product_name")
                .HasMaxLength(200);

            builder.Property(o => o.Quantity)
                .HasColumnName("quantity");

            builder.OwnsOne(o => o.UnitPrice, p =>
            {
                p.Property(x => x.Amount).HasColumnName("unit_price").HasPrecision(18, 6);
                p.Property(x => x.Currency).HasColumnName("unit_price_currency").HasMaxLength(3);
            });

            // Audit fields
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

            // Index 
            builder.HasIndex(o => o.OrderId);
        }
    }
}
