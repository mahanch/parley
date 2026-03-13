using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Parley.Application._Shared.DTOs;

namespace Parley.Api.Filters;

public class BaseResponseFilter : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult objectResult &&
            objectResult.Value is BaseResponse baseResponse &&
            !baseResponse.IsSuccess)
        {
            int statusCode = GetStatusCode(baseResponse.ErrorType);
            objectResult.StatusCode = statusCode;
            context.HttpContext.Response.StatusCode = statusCode;
        }
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