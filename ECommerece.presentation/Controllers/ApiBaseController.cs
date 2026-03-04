using ECommerece.Shared.CommonResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ECommerece.presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiBaseController : ControllerBase
{
    // handle result without value
    // if the result is success, return no content 204
    // if result is failure, return problem with status code and error details

    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return NoContent();
        }
        else
        {
            return HandleProblem(result.Errors);
        }
    }

    // handle result with value
    protected ActionResult<Tvalue> HandleResult<Tvalue>(Result<Tvalue> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return HandleProblem(result.Errors);
        }
    }

    private ActionResult HandleProblem(IReadOnlyList<Error> errors)
    {
        // if no error, return 500
        if (errors.Count == 0)
        {
            return Problem(
                title: "Unknown error",
                statusCode: 500
            );
        }
        // if all errors are validation error, handle as validation error
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            return HandleValidationErrors(errors);
        }
        // if one error, handle as single error
        return HandleSingleError(errors[0]);
    }

    private ActionResult HandleSingleError(Error error)
    {
        return Problem(
            title: error.Code,
            detail: error.Description,
            type: error.Type.ToString(),
            statusCode: MapErrorTypeToStatusCode(error.Type)
            );
    }

    private static int MapErrorTypeToStatusCode(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Failure => 500,
            ErrorType.Validation => 400,
            ErrorType.NotFound => 404,
            ErrorType.Unauthorized => 401,
            ErrorType.Forbidden => 403,
            ErrorType.InvalidCredentials => 401,
            _ => 500
        };
    }

    private ActionResult HandleValidationErrors(IReadOnlyList<Error> errors)
    {
        var modelState = new ModelStateDictionary();
        foreach (var error in errors)
        {
            modelState.AddModelError(error.Code, error.Description);
        }
        return ValidationProblem(modelState);
    }
}

