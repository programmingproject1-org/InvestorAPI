using InvestorApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InvestorApi.Filters
{
    internal sealed class InvalidTradeExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is InvalidTradeException error)
            {
                var response = new
                {
                    Message = error.Message
                };

                context.HttpContext.Response.StatusCode = 400;
                context.HttpContext.Response.Headers.Clear();
                context.Result = new JsonResult(response);
            }
        }
    }
}
