using dotnet_boilderplate.SharedKernel.Common;
using dotnet_boilderplate.SharedKernel.Results;
using NetTopologySuite.Geometries;
using vrp_demo.Domains.Enums;
using vrp_demo.Domains.ValueObjects;
using vrp_demo.Features.Commands.Jobs.CreateJob;

namespace vrp_demo.Domains.Aggregates
{
    public class Job : BaseEntity<Guid>
    {
        public JobCode Code { get; private set; }

        public JobStatus JobStatus { get; private set; }

        public JobType JobType { get; private set; }

        public string Description { get; private set; } = string.Empty;

        public List<Entities.Task> Tasks { get; private set; }

        private Job() { }

        public static Result<Job> CreateJob(JobCode jobCode, JobType jobType, string description, IEnumerable<CreateTaskRequest> taskReqs)
        {
            var job = new Job()
            {
                Id = Guid.NewGuid(),
                Code = jobCode,
                JobType = jobType,
                Description = description,
                JobStatus = JobStatus.New
            };

            switch (jobType)
            {
                case JobType.Service:
                    {
                        foreach (var taskReq in taskReqs)
                        {
                            var location = new Point(taskReq.Lng, taskReq.Lat);

                            var task = Entities.Task.CreateBaseTask(
                                taskReq.Name,
                                taskReq.Notes ?? string.Empty,
                                taskReq.Address,
                                location,
                                taskReq.ServiceTime,
                                taskReq.StartDt,
                                taskReq.EndDt,
                                taskReq.TaskType);

                            task.SetJobId(job.Id);

                            if (taskReq.SkillIds != null && taskReq.SkillIds.Any())
                            {
                                task.SetRequiredSkills(taskReq.SkillIds);
                            }

                            job.Tasks.Add(task);
                        }
                    }
                    break;
                case JobType.Shipment:
                    {
                        // this type of job should have exactly two tasks (pickup, delivery)
                        // no implement check here at first because the validator does the job
                        var pickUp = taskReqs.Where(tr => tr.ShipmentType.HasValue && tr.ShipmentType == ShipmentType.Pick).FirstOrDefault();
                        var delivery = taskReqs.Where(tr => tr.ShipmentType.HasValue && tr.ShipmentType == ShipmentType.Drop).FirstOrDefault();

                        if (pickUp == null || delivery == null) break;

                        var pickUpLoc = new Point(pickUp.Lng, pickUp.Lat);
                        var deliveryLoc = new Point(delivery.Lng, delivery.Lat);

                        var pickUpTask = Entities.Task.CreateBaseTask(
                                pickUp.Name,
                                pickUp.Notes ?? string.Empty,
                                pickUp.Address,
                                pickUpLoc,
                                pickUp.ServiceTime,
                                pickUp.StartDt,
                                pickUp.EndDt,
                                pickUp.TaskType);

                        var deliveryTask = Entities.Task.CreateBaseTask(
                                delivery.Name,
                                delivery.Notes ?? string.Empty,
                                delivery.Address,
                                deliveryLoc,
                                delivery.ServiceTime,
                                delivery.StartDt,
                                delivery.EndDt,
                                delivery.TaskType);

                        pickUpTask.SetJobId(job.Id);
                        deliveryTask.SetJobId(job.Id);

                        if (pickUp.SkillIds != null && pickUp.SkillIds.Any())
                        {
                            pickUpTask.SetRequiredSkills(pickUp.SkillIds);
                        }

                        if (delivery.SkillIds != null && delivery.SkillIds.Any())
                        {
                            deliveryTask.SetRequiredSkills(delivery.SkillIds);
                        }

                        pickUpTask.SetReferenceId(deliveryTask.Id);
                        deliveryTask.SetReferenceId(pickUpTask.Id);

                        job.Tasks.Add(pickUpTask);
                        job.Tasks.Add(deliveryTask);
                    }
                    break;
                default:
                    break;
            }

            if (job.Tasks.Count <= 0) return Result.Failure<Job>(Error.Validation("Job must have at least one task"));

            return Result.Success(job);
        }
    }
}
