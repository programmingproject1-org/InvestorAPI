﻿using InvestorApi.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InvestorApi.Asx
{
    /// <summary>
    /// This class is used by the startup class to register and initialize the components in this library.
    /// </summary>
    public static class AsxModule
    {
        /// <summary>
        /// Regsiteres the services in this library in the applications IOC container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureServices(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Register the provider in the dependency injection container.
            services.AddScoped<IMarketInfoProvider, AsxMarketInfoProvider>();
            services.AddScoped<IShareInfoProvider, AsxShareInfoProvider>();
        }
    }
}
