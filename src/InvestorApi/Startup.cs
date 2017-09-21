using InvestorApi.Asx;
using InvestorApi.Domain;
using InvestorApi.Filters;
using InvestorApi.Repositories;
using InvestorApi.Security;
using InvestorApi.Swagger;
using InvestorApi.Yahoo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace InvestorApi
{
    /// <summary>
    /// The ASP.NET MVC startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Configures the service container.
        /// </summary>
        /// <param name="services">The service container.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc(options =>
            {
                options.InputFormatters.RemoveType<JsonPatchInputFormatter>();
                options.OutputFormatters.RemoveType<StringOutputFormatter>();
                options.Filters.Add(new ParameterValidationFilter());
                options.Filters.Add(new ModelStateValidationFilter());
                options.Filters.Add(new EntityNotFoundExceptionFilter());
                options.Filters.Add(new InvalidTradeExceptionFilter());
                options.Filters.Add(new ValidationExceptionFilter());
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = JwtSettings.SecurtyKey,
                    ValidIssuer = JwtSettings.Issuer,
                    ValidAudiences = new[] { JwtSettings.InvestorAudience, JwtSettings.AdministratorAudience }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    AuthorizationPolicies.Administrators,
                    policy => policy.RequireClaim(JwtRegisteredClaimNames.Aud, JwtSettings.AdministratorAudience));
            });

            services.AddSwaggerGen(options =>
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
            });

            DomainModule.ConfigureServices(services);
            RepositoriesModule.ConfigureServices(services);
            AsxModule.ConfigureServices(services);
            YahooModule.ConfigureServices(services);

            services.AddDbContext<DataContext>(ConfigureDbContext);
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application builder instance.</param>
        /// <param name="env">The hosting environment.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseAuthentication();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{SwaggerConstants.InvestorsGroup}/swagger.json", SwaggerConstants.InvestorsTitle);
                options.SwaggerEndpoint($"/swagger/{SwaggerConstants.AdministratorsGroup}/swagger.json", SwaggerConstants.AdministratorsTitle);
                options.DocExpansion("list");
            });
        }

        /// <summary>
        /// Configures the database context.
        /// </summary>
        /// <param name="options">The database context builder options.</param>
        protected virtual void ConfigureDbContext(DbContextOptionsBuilder options)
        {
            RepositoriesModule.ConfigureDbContext(options);
        }
    }
}
