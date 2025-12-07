using dotnet_boilderplate.SharedKernel.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_boilderplate.ServiceDefaults.Extensions
{
    public static class ResultExtensions
    {
        public static IResult Match<T>(
            this Result<T> result,
            Func<T, IResult> onSuccess,
            Func<Error, IResult> onFailure)
        {
            return result.IsSuccess
                ? onSuccess(result.Value)
                : onFailure(result.Error);
        }

        public static IResult Match(
            this Result result,
            Func<IResult> onSuccess,
            Func<Error, IResult> onFailure)
        {
            return result.IsSuccess
                ? onSuccess()
                : onFailure(result.Error);
        }

        public static IResult ToProblemDetails(this Error error)
        {
            var problem = new ProblemDetails
            {
                Title = error.Code,
                Detail = error.Message,
                Status = error.Code switch
                {
                    "Validation" => 400,
                    "NotFound" => 404,
                    "Conflict" => 409,
                    "Unauthorized" => 401,
                    "Forbidden" => 403,
                    _ => 400
                }
            };

            return Results.Problem(problem);
        }
    }
}
