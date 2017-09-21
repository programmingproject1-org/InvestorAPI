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
    internal sealed class ParameterValidationFilter : IActionFilter
    {
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

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private static void ValidateParameters(ActionExecutingContext context)
        {
            var parametersWithValidators = GetParametersWithValidators(context);
            var argumentstoValidate = parametersWithValidators.Select(p => GetArgumentValue(p.Key, context.ActionArguments));

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