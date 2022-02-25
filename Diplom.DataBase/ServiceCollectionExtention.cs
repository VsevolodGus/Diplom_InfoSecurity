using System;
using System.Collections.Generic;
using Diplom.DataBase.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Diplom.DataBase
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection AddEfRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DiplomDbContext>(
                options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("Default"));
                },
                ServiceLifetime.Transient
            );

            //для доступа к базе
            services.AddScoped<Dictionary<Type, DiplomDbContext>>();
            services.AddSingleton<DbContextFactory>();

            services.AddSingleton<IFileRepository, FileRepository>();


            return services;
        }
    }
}
