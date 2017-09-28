using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.ComponentModel.DataAnnotations;

namespace InvestorApi.Filters
{
    /// <summary>
    /// Automatically converts exceptions of type <see cref="ValidationException"/> into HTTP 404 responses.
    /// </summary>
    internal sealed class ValidationExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// Called after an action has thrown an <see cref="Exception" />.
        /// </summary>
        /// <param name="context">The <see cref="ExceptionContext" />.</param>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException error)
            {
                context.Result = new BadRequestObjectResult(new { Message = error.Message });
            }
        }
    }
}
