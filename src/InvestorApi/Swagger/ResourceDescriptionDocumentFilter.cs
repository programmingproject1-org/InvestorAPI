using System;
using System.IO;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace InvestorApi.Swagger
{
    /// <summary>
    /// An implementation of <see cref="IDocumentFilter" /> which adds documentation sections from embedded resources.
    /// </summary>
    public sealed class ResourceDescriptionDocumentFilter : IDocumentFilter
    {
        private readonly string _resourcePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceDescriptionDocumentFilter" /> class.
        /// </summary>
        /// <param name="resourcePath">The resource path.</param>
        public ResourceDescriptionDocumentFilter(string resourcePath)
        {
            _resourcePath = resourcePath ?? throw new ArgumentNullException(nameof(resourcePath));
        }

        /// <summary>
        /// Inserts the document into the swagger specification.
        /// </summary>
        /// <param name="swaggerDoc">The swagger document.</param>
        /// <param name="context">The context.</param>
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
            Assembly assembly = typeof(ResourceDescriptionDocumentFilter).Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(_resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}