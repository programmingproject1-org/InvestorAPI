using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InvestorApi.Filters
{

    internal sealed class ValidationExceptionFilter : IExceptionFilter
    {
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
