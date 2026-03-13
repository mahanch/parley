using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Server.HttpSys;
using Parley.Application._Shared.DTOs;
using Parley.Domain._Shared.Exceptions;

namespace Parley.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var baseResult = exception switch
        {
            FluentValidation.ValidationException fluentValidationEx =>
                BaseResponse.ValidationError(
                    fluentValidationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        )
                ),
                
            // Add your custom exceptions here when you create them:
            BadRequestException badRequestEx =>
                BaseResponse.Failure(badRequestEx.Message, ErrorType.BadRequest),
            NotFoundException notFoundEx =>
                BaseResponse.NotFound(notFoundEx.Message),
            UnAuthorizedException unauthorizedEx =>
                BaseResponse.Unauthorized(unauthorizedEx.Message),
            
            _ => BaseResponse.InternalServerError(
                _env.IsDevelopment() ? exception.Message : "An internal error occurred."
                )
        };

        httpContext.Response.StatusCode = GetStatusCode(baseResult.ErrorType);
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(baseResult, cancellationToken);

        return true; // Exception handled
    }

    private static int GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.BadRequest => StatusCodes.Status400BadRequest,
        ErrorType.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };
}
