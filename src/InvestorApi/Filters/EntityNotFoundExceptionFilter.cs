using InvestorApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace InvestorApi.Filters
{
    /// <summary>
    /// Automatically converts exceptions of type <see cref="EntityNotFoundException"/> into HTTP 404 responses.
    /// </summary>
    internal sealed class EntityNotFoundExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// Called after an action has thrown an <see cref="Exception" />.
        /// </summary>
        /// <param name="context">The <see cref="ExceptionContext" />.</param>
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
