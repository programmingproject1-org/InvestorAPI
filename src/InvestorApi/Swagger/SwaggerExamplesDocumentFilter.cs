using System;
using System.IO;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;
using System.Linq;

namespace InvestorApi.Swagger
{
    internal sealed class SwaggerExamplesDocumentFilter : IOperationFilter
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

            if (operation.Parameters != null)
            {
                var bodySchemas = operation.Parameters.OfType<BodyParameter>().Select(p => p.Schema.Ref);
                var responseSchemas = operation.Responses.Where(p => p.Value.Schema?.Ref != null).Select(p => p.Value.Schema.Ref);
                var schemas = bodySchemas.Union(responseSchemas);

                foreach (var schemaRef in schemas)
                {
                    SetProperties(context.SchemaRegistry, schemaRef);
                }
            }
        }

        private void SetProperties(ISchemaRegistry schemaRegistry, string schemaRef)
        {
            var schemaName = schemaRef.Split('/').Last();
            if (!schemaRegistry.Definitions.ContainsKey(schemaName))
            {
                return;
            }

            var schema = schemaRegistry.Definitions[schemaName];

            if (schema.Properties != null)
            {
                foreach (var property in schema.Properties)
                {
                    switch (property.Key)
                    {
                        case "side":
                            property.Value.Example = "Buy";
                            break;
                        case "email":
                            property.Value.Example = "user@host.com";
                            break;
                        case "password":
                            property.Value.Example = "a1b2c3d4e5f6";
                            break;
                        case "symbol":
                            property.Value.Example = "ANZ";
                            break;
                        case "quantity":
                            property.Value.Example = 100;
                            break;
                    }

                    if (property.Value.Ref != null)
                    {
                        SetProperties(schemaRegistry, property.Value.Ref);
                    }
                }
            }
        }
    }
}
