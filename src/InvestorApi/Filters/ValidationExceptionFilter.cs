using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
                var response = new
                {
                    Message = error.Message,
                    ValidationErrors = new[]
                    {
                        new
                        {
                            PropertyName = error.ValidationResult.MemberNames.FirstOrDefault(),
                            Message = error.ValidationResult.ErrorMessage,
                            RuleName = error.ValidationAttribute?.GetType().Name.Replace("Attribute", string.Empty)
                        }
                    }
                };

                context.HttpContext.Response.StatusCode = 400;
                context.HttpContext.Response.Headers.Clear();
                context.Result = new JsonResult(response);
            }
        }
    }
}
