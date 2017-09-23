using InvestorApi.Asx;
using InvestorApi.Domain;
using InvestorApi.Filters;
using InvestorApi.Repositories;
using InvestorApi.Security;
using InvestorApi.Swagger;
using InvestorApi.Swagger.UI;
using InvestorApi.Yahoo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        /// <param name="env">The hosting environment.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Environment = env;
        }

        /// <summary>
        /// Gets the hosting environment.
        /// </summary>
        public IHostingEnvironment Environment { get; }

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
            // Enabled CORS to allow access from browser applications on different domains.
            services.AddCors();

            // Configure the API and JSON behaviour.
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

            // Configure JWT authentication.
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

            // Configure Authorization to prevent investor users from using admin features.
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    AuthorizationPolicies.Administrators,
                    policy => policy.RequireClaim(JwtRegisteredClaimNames.Aud, JwtSettings.AdministratorAudience));
            });

            // Enable Swagger and Swagger UI to make exploration of the API easier.
            services.AddSwaggerGen(SwaggerConfig.Configure);

            // Register the components from all modules in the dependency injection container.
            DomainModule.ConfigureServices(services);
            RepositoriesModule.ConfigureServices(services);
            AsxModule.ConfigureServices(services);
            YahooModule.ConfigureServices(services);

            // Create the database context. In the development environment, we just use an in-memory database.
            if (Environment.IsDevelopment())
            {
                RepositoriesModule.ConfigureInMemoryDbContext(services);
            }
            else
            {
                RepositoriesModule.ConfigurePostgresDbContext(services);
            }
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

            // Enabled CORS to allow access from browser applications on different domains.
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseMvc();

            // Enable Swagger and Swagger UI to make exploration of the API easier.
            app.UseSwagger();
            app.UseSwaggerUI(SwaggerConfig.ConfigureUI);
            app.UseSwaggerBearerAuthorization();
        }
    }
}
