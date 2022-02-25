using Diplom.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace Diplom.DataBase
{
    public class DiplomDbContext : DbContext
    {
        public DbSet<FileModel> FileModels { get; }

        public DiplomDbContext(DbContextOptions<DiplomDbContext> options) : base(options)
        { }

    }
}
