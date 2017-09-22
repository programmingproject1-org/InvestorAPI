using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace InvestorApi.Filters
{
    /// <summary>
    /// The filter automatically aborts a request and returns an HTTP 400 response if the
    /// URL prameters have any data annotation valiation attributes which fail validation.
    /// </summary>
    internal sealed class ParameterValidationFilter : IActionFilter
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

            ValidateParameters(context);

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

        private static void ValidateParameters(ActionExecutingContext context)
        {
            // Get all parameters which have at least one validation attribute assigned.
            var parametersWithValidators = GetParametersWithValidators(context);

            // Get the corresponding value. This is a bit tricky because the ActionArguments collection
            // does not contain the parameter value at all if is wasn't supplied by the caller.
            var argumentstoValidate = parametersWithValidators.Select(p => GetArgumentValue(p.Key, context.ActionArguments));

            // Now check all valication attributes with the parameter value.
            foreach (var argument in argumentstoValidate)
            {
                var validators = parametersWithValidators[argument.Key];
                var errors = validators.Where(v => !v.IsValid(argument.Value));

                foreach (var error in errors)
                {
                    context.ModelState.AddModelError(argument.Key, error.FormatErrorMessage(argument.Key));
                }
            }
        }

        private static IDictionary<string, List<ValidationAttribute>> GetParametersWithValidators(ActionExecutingContext context)
        {
            return context.ActionDescriptor.Parameters
                .OfType<ControllerParameterDescriptor>()
                .Select(p => new
                {
                    Name = p.Name,
                    Validators = p.ParameterInfo.GetCustomAttributes<ValidationAttribute>(true).ToList()
                })
                .Where(p => p.Validators.Any())
                .ToDictionary(
                    p => p.Name,
                    p => p.Validators);
        }

        private static KeyValuePair<string, object> GetArgumentValue(string key, IDictionary<string, object> arguments)
        {
            if (arguments.ContainsKey(key))
            {
                return arguments.FirstOrDefault(a => a.Key == key);
            }

            return new KeyValuePair<string, object>(key, null);
        }
    }
}