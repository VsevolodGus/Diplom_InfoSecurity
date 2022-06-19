using DiplomInfo.DataBase.InterfaceRepository;
using DiplomInfo.DataBase.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiplomInfo.DataBase
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configure)
        {
            // при переводе на базу, сделать Scoped
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IFileRepository, FileRepository>();

            return services;
        }
    }
}
