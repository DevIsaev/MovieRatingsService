using Domain.ValueObject.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MovieRatingsService.WebHost.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext ctx)
        {
            var status = ctx.Exception switch
            {
                EntityNotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedOperationException => StatusCodes.Status403Forbidden,
                //ConflictException or DomainOperationException => StatusCodes.Status409Conflict,
                DomainException or ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            ctx.Result = new ObjectResult(new
            {
                Error = status == 500 ? "Внутренняя ошибка сервера" : ctx.Exception.Message,
                Status = status
            })
            { StatusCode = status };

            ctx.ExceptionHandled = true;
        }
    }
}
