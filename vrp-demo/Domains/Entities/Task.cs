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

        #region ServiceProperty

        #endregion

        #region ShipmentProperty

        #endregion
    }
}
