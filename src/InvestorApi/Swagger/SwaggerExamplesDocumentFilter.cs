using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace InvestorApi.Swagger
{
    /// <summary>
    /// This operation filter adds useful sample values to properties in Swagger specifications.
    /// </summary>
    /// <seealso cref="Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter" />
    internal sealed class SwaggerExamplesDocumentFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The operation filter context.</param>
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

        /// <summary>
        /// Sets the property sample values.
        /// </summary>
        /// <param name="schemaRegistry">The schema registry.</param>
        /// <param name="schemaRef">The reference of the schema to process.</param>
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
                        case "displayName":
                            property.Value.Example = "John Doe";
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
                        case "level":
                            property.Value.Example = "Administrator";
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
