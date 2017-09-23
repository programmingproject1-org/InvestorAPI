using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace InvestorApi.Swagger.UI
{
    internal static class SwaggerExtensions
    {
        public static IApplicationBuilder UseSwaggerBearerAuthorization(this IApplicationBuilder app)
        {
            var fileServerOptions = new FileServerOptions
            {
                RequestPath = $"/swagger/customizations",
                FileProvider = new EmbeddedFileProvider(typeof(SwaggerExtensions).GetTypeInfo().Assembly, "InvestorApi.Swagger.UI")
            };

            fileServerOptions.StaticFileOptions.ContentTypeProvider = new FileExtensionContentTypeProvider();
            app.UseFileServer(fileServerOptions);

            return app;
        }
    }
}
