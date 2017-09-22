using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace InvestorApi.Filters
{
    /// <summary>
    /// The filter automatically aborts a request and returns an HTTP 400 response if the
    /// request body has any data annotation valiation attributes which fail validation.
    /// </summary>
    internal sealed class ModelStateValidationFilter : IActionFilter
    {
        /// <summary>
        /// Called before the action executes, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutingContext" />.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        /// <summary>
        /// Called after the action executes, before the action result.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutedContext" />.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
