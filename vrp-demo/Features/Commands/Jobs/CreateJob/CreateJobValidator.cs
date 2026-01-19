using FluentValidation;
using vrp_demo.Domains.Enums;

namespace vrp_demo.Features.Commands.Jobs.CreateJob
{
    public class CreateJobValidator : AbstractValidator<CreateJobRequest>
    {
        public CreateJobValidator()
        {
            RuleFor(e => e.JobType)
                .NotEqual(JobType.Unknown)
                .IsInEnum();

            RuleForEach(e => e.Tasks).ChildRules(t =>
                {
                    t.RuleFor(task => task.Name).NotNull();
                    t.RuleFor(task => task.Lat)
                        .NotEmpty()
                        .NotNull()
                        .GreaterThan(-90)
                        .LessThanOrEqualTo(90);
                    t.RuleFor(task => task.Lng)
                        .NotEmpty()
                        .NotNull()
                        .GreaterThan(-180)
                        .LessThanOrEqualTo(180);
                    t.RuleFor(task => task.TaskType).IsInEnum();
                });
        }
    }
}
