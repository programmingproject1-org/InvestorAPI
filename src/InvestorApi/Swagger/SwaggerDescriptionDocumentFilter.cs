using System;
using System.IO;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace InvestorApi.Swagger
{
    /// <summary>
    /// This document filter adds a markdown file from the embedded resources to the Swagger specification.
    /// </summary>
    internal sealed class SwaggerDescriptionDocumentFilter : IDocumentFilter
    {
        private readonly string _resourcePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerDescriptionDocumentFilter"/> class.
        /// </summary>
        /// <param name="resourcePath">The resource path.</param>
        public SwaggerDescriptionDocumentFilter(string resourcePath)
        {
            _resourcePath = resourcePath ?? throw new ArgumentNullException(nameof(resourcePath));
        }

        /// <summary>
        /// Applies the specified swagger document.
        /// </summary>
        /// <param name="swaggerDoc">The swagger document.</param>
        /// <param name="context">The document filter context.</param>
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

        /// <summary>
        /// Loads the document from the embedded resources.
        /// </summary>
        /// <returns>The document text in markdown format.</returns>
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