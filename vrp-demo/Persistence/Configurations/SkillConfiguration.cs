using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using vrp_demo.Domains.ValueObjects;

namespace vrp_demo.Persistence.Configurations
{
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("skill");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("id");

            builder.Property(s => s.Name)
                .HasColumnName("name")
                .HasColumnType("text");

            builder.Property(s => s.Description)
                .HasColumnName("description");
        }
    }
}
