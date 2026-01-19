using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using vrp_demo.Domains.Enums;

namespace vrp_demo.Persistence.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Domains.Entities.Task>
    {
        public void Configure(EntityTypeBuilder<Domains.Entities.Task> builder)
        {
            var taskTypeConverter = new EnumToStringConverter<TaskType>();
            var taskStatusConverter = new EnumToStringConverter<Domains.Enums.TaskStatus>();
            var shipmentTypeConverter = new EnumToStringConverter<ShipmentType>();

            builder.ToTable("task");

            builder.HasKey(d => d.Id);

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType("text");

            builder.Property(x => x.Notes)
                .HasColumnName("notes")
                .HasColumnType("text");

            builder.Property(x => x.Address)
                .HasColumnName("address")
                .HasColumnType("text");

            builder.Property(x => x.TaskType)
                .HasColumnName("task_type")
                .HasColumnType("text")
                .HasConversion(taskTypeConverter);

            builder.Property(p => p.Location)
                .HasColumnName("location")
                .HasColumnType("geography (point, 4326)");

            builder.Property(o => o.StartDt)
                .HasColumnName("start_dt")
                .HasColumnType("timestamptz");

            builder.Property(o => o.EndDt)
                .HasColumnName("end_dt")
                .HasColumnType("timestamptz");

            builder.Property(p => p.RequiredSkills)
                .HasColumnName("required_skills")
                .HasConversion(
                    from => JsonConvert.SerializeObject(from),
                    reverse => JsonConvert.DeserializeObject<List<Guid>>(reverse) ?? new List<Guid>()
                );

            builder.Property(o => o.ServiceTime)
                .HasColumnName("service_time");

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasColumnType("text")
                .HasConversion(taskStatusConverter);

            builder.Property(o => o.JobId)
                .HasColumnName("job_id");

            builder.Property(o => o.ReferenceTaskId)
                .HasColumnName("reference_task_id")
                .IsRequired(false);

            builder.Property(o => o.Capacity)
                .HasColumnName("capacity")
                .IsRequired(false);

            builder.Property(o => o.ShipmentType)
                .HasColumnName("shipment_type")
                .HasColumnType("text")
                .HasConversion(shipmentTypeConverter)
                .IsRequired(false);

            builder.Property(o => o.Distance)
                .HasColumnName("distance")
                .IsRequired(false);

            builder.Property(o => o.WaitingTime)
                .HasColumnName("waiting_time")
                .IsRequired(false);

            builder.Property(o => o.ExpectedArrival)
                .HasColumnName("expected_arrival")
                .HasColumnType("timestamptz")
                .IsRequired(false);

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
