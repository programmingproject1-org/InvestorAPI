using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Swagger
{
    internal sealed class SwaggerBearerAuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.ApiDescription.ActionDescriptor.FilterDescriptors.Any(filter => filter.Filter is AuthorizeFilter))
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<IParameter>();
                }

                var parameter = new NonBodyParameter
                {
                    Name = "Authorization",
                    Description = "Access Token",
                    Type = "string",
                    Required = true,
                    @In = "header",
                    @Default = "Bearer "
                };

                operation.Parameters.Insert(0, parameter);
            }
        }
    }
}
