using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;

namespace ProductCatalogue.Api.Extensions;

public static class ResultExtensions
{
    public static ActionResult<T> ToActionResult<T>(this Result<T> result) =>
        result.IsSuccess
            ? new OkObjectResult(result.Value)
            : ErrorResult(result.Error!);

    public static ActionResult ToActionResult(this Result result) =>
        result.IsSuccess
            ? new NoContentResult()
            : ErrorResult(result.Error!);

    private static ObjectResult ErrorResult(Error error) => error.Type switch
    {
        ErrorType.NotFound     => new NotFoundObjectResult(ToProblem(error)),
        ErrorType.Conflict     => new ConflictObjectResult(ToProblem(error)),
        ErrorType.Validation   => new BadRequestObjectResult(ToProblem(error)),
        ErrorType.Unauthorized => new UnauthorizedObjectResult(ToProblem(error)),
        _ => new ObjectResult(ToProblem(error)) { StatusCode = 500 }
    };

    private static ProblemDetails ToProblem(Error error) => new()
    {
        Detail = error.Message
    };
}
