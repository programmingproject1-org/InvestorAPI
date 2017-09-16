using System;
using System.IO;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace InvestorApi.Swagger
{
    internal sealed class SwaggerDescriptionDocumentFilter : IDocumentFilter
    {
        private readonly string _resourcePath;

        public SwaggerDescriptionDocumentFilter(string resourcePath)
        {
            _resourcePath = resourcePath ?? throw new ArgumentNullException(nameof(resourcePath));
        }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            if (swaggerDoc == null)
            {
                throw new ArgumentNullException(nameof(swaggerDoc));
            }

            if (swaggerDoc.Info.Description == null)
            {
                swaggerDoc.Info.Description = LoadResource();
            }
        }

        private string LoadResource()
        {
            Assembly assembly = typeof(SwaggerDescriptionDocumentFilter).Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(_resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}