using ECommerece.Service.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.CustomMiddleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;


    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
            await HandleNotFoundEndPointAsync(context);
        }
        catch (Exception ex)
        {
            // Log the exception (you can use a logging framework here)
            _logger.LogError(ex ,"Somthing went wrong");
            //return a generic error response
            var problem = new ProblemDetails()
            {
                Title = "An error occurred while processing your request.",
                Detail = ex.Message,
                Instance = context.Request.Path,
                Status = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                }

            };
            context.Response.StatusCode = problem.Status.Value;
            await context.Response.WriteAsJsonAsync(problem);
        }
    }

    private async Task HandleNotFoundEndPointAsync(HttpContext context)
    {
        if (context.Response.StatusCode == StatusCodes.Status404NotFound)
        {
            var response = new ProblemDetails()
            {
                Title = "Resource Not Found",
                Detail = $"The requested resource at {context.Request.Path} was not found.",
                Status = StatusCodes.Status404NotFound,
                Instance = context.Request.Path
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}


