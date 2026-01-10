using dotnet_boilderplate.SharedKernel.Common;
using NetTopologySuite.Geometries;
using vrp_demo.Domains.Enums;

namespace vrp_demo.Domains.Entities
{
    public class Task : BaseEntity<Guid>
    {
        public string Name { get; private set; } = string.Empty;

        public string Notes { get; private set; } = string.Empty;

        public TaskType TaskType { get; private set; }

        public string Address { get; private set; } = string.Empty;

        public Point Location { get; private set; }

        /// <summary>
        /// RequestStartTime: Early time acceptance
        /// </summary>
        public DateTime StartDt { get; private set; }

        /// <summary>
        /// RequestEndTime: Latest time acceptance
        /// </summary>
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

        public Guid? ReferenceTaskId { get; private set; }
        #endregion

        #region AfterOptimization
        public DateTime? ExpectedArrival { get; private set; }

        public double? Distance { get; private set; }

        public long? WaitingTime { get; private set; }
        #endregion

        private Task() { }
    
        public static Task CreateBaseTask(string name, string notes, string address, Point loc, int serviceTime, DateTime early, DateTime late, TaskType? taskType)
        {
            var task = new Task()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Notes = notes,
                Address = address,
                Location = loc,
                StartDt = early,
                EndDt = late,
                Status = Enums.TaskStatus.New,
                ServiceTime = serviceTime,
            };

            if (taskType != null) task.TaskType = taskType.Value;

            return task;
        }

        public void SetJobId(Guid jobId)
        {
            JobId = jobId;
        }

        public void SetRequiredSkills(IEnumerable<Guid> skillIds)
        {
            RequiredSkills = [.. skillIds];
        }

        public void SetStatus(Enums.TaskStatus status)
        {  
            Status = status;  
        }

        public void SetServiceTime(int serviceTime)
        {
            ServiceTime = serviceTime;
        }

        #region Shipments
        public void SetCapacity(int capacity)
        {
            Capacity = capacity; 
        }

        public void SetShipmentType(ShipmentType shipmentType)
        {
            if (TaskType == TaskType.Service) return;

            ShipmentType = shipmentType;
        }

        public void SetReferenceId(Guid refId)
        {
            if (TaskType == TaskType.Service) return;

            ReferenceTaskId = refId;
        }
        #endregion
    }
}
