using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Diplom.DataBase
{
    public class DbContextFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public DbContextFactory(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public DiplomDbContext Create(Type repositoryType)
        {
            var services = httpContextAccessor.HttpContext.RequestServices;

            var dbContexts = services.GetService<Dictionary<Type, DiplomDbContext>>();
            if (!dbContexts.ContainsKey(repositoryType))
                dbContexts[repositoryType] = services.GetService<DiplomDbContext>();

            return dbContexts[repositoryType];
        }
    }
}
