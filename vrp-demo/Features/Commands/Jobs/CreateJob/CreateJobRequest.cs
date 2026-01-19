using vrp_demo.Domains.Aggregates;
using vrp_demo.Domains.Enums;

namespace vrp_demo.Features.Commands.Jobs.CreateJob
{
    public record CreateJobRequest(string? Description, JobType JobType, IEnumerable<CreateTaskRequest> Tasks);

    public record CreateTaskRequest(string Name, string? Notes, string Address, double Lng, double Lat, TaskType TaskType, DateTime StartDt, DateTime EndDt, IEnumerable<Guid> SkillIds, int ServiceTime, ShipmentType? ShipmentType, int? Capacity);

    public record CreateJobResponse(Job Job);
}
