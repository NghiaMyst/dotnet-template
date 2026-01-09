using dotnet_boilderplate.SharedKernel.Common;
using NetTopologySuite.Geometries;
using vrp_demo.Domains.Enums;

namespace vrp_demo.Domains.Entities
{
    public class Task : BaseEntity<Guid>
    {
        public string Name { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public TaskType TaskType { get; private set; }

        public string Address { get; private set; } = string.Empty;

        public Point Location { get; private set; }

        public DateTime StartDt { get; private set; }

        public DateTime EndDt { get; private set; }

        public List<Guid> RequiredSkills { get; private set; } = [];

        public Guid JobId { get; private set; }

        public Enums.TaskStatus Status { get; private set; }

        public int ServiceTime { get; private set; }

        #region ServiceProperty

        #endregion

        #region ShipmentProperty
        public int? Capacity { get; private set; }

        public ShipmentType? ShipmentType { get; private set; }
        #endregion

        #region AfterOptimization
        public DateTime? ExpectedArrival { get; private set; }

        public double? Distance { get; private set; }
        #endregion
    }
}
