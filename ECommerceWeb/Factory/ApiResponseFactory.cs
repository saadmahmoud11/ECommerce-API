using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Factory;

public static class ApiResponseFactory
{
    public static IActionResult GenerateApiValidationResponse(ActionContext actionContext)
    {
        var errors = actionContext.ModelState
        .Where(e => e.Value.Errors.Count > 0)
        .ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        );
        var problem = new ProblemDetails()
        {
            Title = "validation errors.",
            Detail = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
            Extensions = { ["errors"] = errors }
        };
        return new BadRequestObjectResult(problem);
    }
}
