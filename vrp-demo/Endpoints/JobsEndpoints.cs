using vrp_demo.Features.Commands.Jobs.CreateJob;

namespace vrp_demo.Endpoints
{
    public static class JobsEndpoints
    {
        public static IEndpointRouteBuilder MapJobsEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapCreateJob();

            return builder;
        }
    }
}
