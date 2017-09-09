using InvestorApi.Domain;
using InvestorApi.Filters;
using InvestorApi.Repositories;
using InvestorApi.Security;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
            services.AddMvc(options =>
            {
                options.InputFormatters.RemoveType<JsonPatchInputFormatter>();
                options.OutputFormatters.RemoveType<StringOutputFormatter>();
                options.Filters.Add(new ModelStateValidationFilter());
                options.Filters.Add(new EntityNotFoundExceptionFilter());
                options.Filters.Add(new ValidationExceptionFilter());
            });

            services.AddCors();

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
                    AuthorizationPolicies.Investors,
                    policy => policy.RequireClaim(JwtRegisteredClaimNames.Aud, JwtSettings.InvestorAudience));

                options.AddPolicy(
                    AuthorizationPolicies.Administrators,
                    policy => policy.RequireClaim(JwtRegisteredClaimNames.Aud, JwtSettings.AdministratorAudience));
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1_0", new Info { Title = "Investor API", Version = "1.0" });
                options.DescribeAllEnumsAsStrings();
                options.OperationFilter<SwaggerBearerAuthorizationOperationFilter>();
                options.IncludeXmlComments(AppContext.BaseDirectory + "InvestorApi.xml");
                options.IncludeXmlComments(AppContext.BaseDirectory + "InvestorApi.Contracts.xml");
                options.DocumentFilter<ResourceDescriptionDocumentFilter>("InvestorApi.Swagger.Documentation.md");
            });

            DomainModule.ConfigureServices(services);
            RepositoriesModule.ConfigureServices(services);

            services.AddDbContext<DataContext>(ConfigureDbContext);
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application builder instance.</param>
        /// <param name="env">The hosting environment.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/v1_0/swagger.json", "Investor API");
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
