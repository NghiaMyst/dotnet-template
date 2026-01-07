using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using vrp_demo.Domains.Aggregates;

namespace vrp_demo.Persistence.Configurations
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.ToTable("driver");

            builder.HasKey(d => d.Id);

            builder.Property(p => p.Name)
                .HasColumnName("name")
                .HasColumnType("text");

            builder.Property(p => p.Address)
                .HasColumnName("address")
                .HasColumnType("text");

            builder.Property(p => p.Lat)
                .HasColumnName("lat");

            builder.Property(p => p.Lng)
                .HasColumnName("lng");

            builder.Property(p => p.Role)
                .HasColumnName("role")
                .HasMaxLength(100);

            builder.Property(p => p.SkillIds)
                .HasColumnName("skill_ids")
                .HasConversion(
                    from => JsonConvert.SerializeObject(from),
                    reverse => JsonConvert.DeserializeObject<List<Guid>>(reverse) ?? new List<Guid>()
                );

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
