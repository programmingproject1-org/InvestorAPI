using InvestorApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InvestorApi.Filters
{
    internal sealed class EntityNotFoundExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is EntityNotFoundException error)
            {
                var response = new
                {
                    Message = error.Message
                };

                context.HttpContext.Response.StatusCode = 404;
                context.HttpContext.Response.Headers.Clear();
                context.Result = new JsonResult(response);
            }
        }
    }
}
