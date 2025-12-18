using dotnet_template.AuthService.Domains.Aggregates;
using dotnet_template.AuthService.Domains.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace dotnet_template.AuthService.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(from => Guid.Parse(from.ToString()), to => UserId.From(to.ToString()));

            builder
                .Property(x => x.Email)
                .HasColumnName("email")
                .HasColumnType("text");

            builder
                .Property(x => x.NormalizedEmail)
                .HasColumnName("normalized_email")
                .HasColumnType("text");

            builder
                .Property(x => x.Email)
                .HasColumnName("email")
                .HasColumnType("text");

            builder
                .Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .HasColumnType("text");

            builder
                .Property(x => x.Roles)
                .HasColumnName("roles")
                .HasConversion(
                    from => JsonConvert.SerializeObject(from),
                    reverse => JsonConvert.DeserializeObject<List<string>>(reverse) ?? new List<string>()
                );

            // TODO: Update to a shared configuration with these configs
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
