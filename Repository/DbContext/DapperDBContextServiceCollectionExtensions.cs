﻿using Microsoft.Extensions.DependencyInjection;
using Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.DbContext
{
    public static class DapperDBContextServiceCollectionExtensions
    {
        public static IServiceCollection AddDapperDBContext<T>(this IServiceCollection services, Action<DapperDBContextOptions> setupAction) where T : DapperDBContext
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddOptions();
            services.Configure(setupAction);
            services.AddScoped<DapperDBContext, T>();
            services.AddScoped<IUnitOfWorkFactory, DapperUnitOfWorkFactory>();

            return services;
        }
    }
}
