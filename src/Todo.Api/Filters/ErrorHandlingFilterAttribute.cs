namespace Todo.Api.Filters;

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
{
    // TODO : Handle more exception on this filter
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        context.Result = new ObjectResult(new { error = exception.Message })
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
        };
        context.ExceptionHandled = true;
    }
}
