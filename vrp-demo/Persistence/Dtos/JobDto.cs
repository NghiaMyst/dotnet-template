using vrp_demo.Domains.Enums;

namespace vrp_demo.Persistence.Dtos
{
    public class JobDto
    {
        public record TaskDto(
            string Name, string Notes, string TaskType, 
            string Address, double Lat, double Lng, 
            DateTime StartDt, DateTime EndDt, List<Guid> RequiredSkills,
            Guid JobId, Domains.Enums.TaskStatus Status, int ServiceTime,
            int? Capacity, string? ShipmentType, Guid? ReferenceTaskId,
            DateTime ExpectedArrival, double Distance, long WaitingTime);
    }
}
