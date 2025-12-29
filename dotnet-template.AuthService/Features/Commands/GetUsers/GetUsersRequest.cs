using dotnet_boilderplate.SharedKernel.Results;
using dotnet_template.AuthService.Domains.Aggregates;
using dotnet_template.AuthService.Projections;

namespace dotnet_template.AuthService.Features.Commands.GetUsers
{
    // TODO: implement filter property if needed
    public record GetUsersRequest(int? PageSize, int? PageNumber);

    public record GetUsersResponse(ResponseList<LiteUserViewModel> Users);
}
