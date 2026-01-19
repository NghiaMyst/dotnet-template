using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using vrp_demo.Domains.Aggregates;
using vrp_demo.Domains.Enums;
using vrp_demo.Domains.ValueObjects;

namespace vrp_demo.Persistence.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            var jobTypeConverter = new EnumToStringConverter<JobType>();
            var jobStatus = new EnumToStringConverter<JobStatus>();


            builder.ToTable("job");

            builder.HasKey(d => d.Id);

            builder.Property(o => o.Description)
                .HasColumnName("description")
                .HasColumnType("text");

            builder.Property(o => o.JobType)
                .HasColumnName("job_type")
                .HasColumnType("text")
                .HasConversion(jobTypeConverter);

            builder.Property(o => o.JobStatus)
                .HasColumnName("job_status")
                .HasColumnType("text")
                .HasConversion(jobStatus);

            builder.Property(o => o.Code)
                .HasColumnName("code")
                .HasColumnType("text")
                .HasConversion(
                    from => from.ToString(),
                    reverse => JobCode.Create(reverse)
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

            // Ignore relation field
            builder.Ignore(o => o.Tasks);
        }
    }
}
