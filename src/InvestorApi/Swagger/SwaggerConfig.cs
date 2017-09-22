using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace InvestorApi.Swagger
{
    internal static class SwaggerConfig
    {
        public static void Configure(SwaggerGenOptions options)
        {
            options.SwaggerDoc(SwaggerConstants.InvestorsGroup, new Info { Title = SwaggerConstants.InvestorsTitle, Version = "1.0" });
            options.SwaggerDoc(SwaggerConstants.AdministratorsGroup, new Info { Title = SwaggerConstants.AdministratorsTitle, Version = "1.0" });

            options.DescribeAllEnumsAsStrings();
            options.OperationFilter<SwaggerExamplesDocumentFilter>();
            options.DocumentFilter<SwaggerDescriptionDocumentFilter>("InvestorApi.Swagger.Documentation.md");

            options.IncludeXmlComments(AppContext.BaseDirectory + "InvestorApi.xml");
            options.IncludeXmlComments(AppContext.BaseDirectory + "InvestorApi.Contracts.xml");

            options.AddSecurityDefinition("Bearer", new ApiKeyScheme()
            {
                Description = "Enter: Bearer {token}",
                Name = "Authorization",
                In = "header",
                Type = "apiKey"
            });
        }

        public static void ConfigureUI(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint($"/swagger/{SwaggerConstants.InvestorsGroup}/swagger.json", SwaggerConstants.InvestorsTitle);
            options.SwaggerEndpoint($"/swagger/{SwaggerConstants.AdministratorsGroup}/swagger.json", SwaggerConstants.AdministratorsTitle);
            options.DocExpansion("list");
        }
    }
}
