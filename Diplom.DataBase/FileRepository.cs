using Diplom.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diplom.DataBase
{
    public class FileRepository
    {
        private readonly DbContextFactory dbContextFactory;

        public FileRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }


        public async Task<FileModel> GetFileById(Guid id)
        {
            var dc = dbContextFactory.Create(typeof(FileRepository));

            return await dc.FileModels.FirstOrDefaultAsync(c=> c.Id == id);
        }


        public async Task<List<FileModel>> GetListFileModels()
        {
            var dc = dbContextFactory.Create(typeof(FileRepository));

            return await dc.FileModels.OrderByDescending(c => c.InsertDate).ToListAsync();
        }

        public async Task<bool> AddFile(FileModel model)
        {
            try
            {
                var dc = dbContextFactory.Create(typeof(FileRepository));

                if (await dc.FileModels.AnyAsync(c=> c.Id == model.Id))
                    return false;

                model.InsertDate = DateTime.UtcNow;
                await dc.FileModels.AddAsync(model);
                await dc.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }

        }
        

        public async Task<bool> UpdateFile(FileModel model)
        {
            try
            {
                var dc = dbContextFactory.Create(typeof(FileRepository));

                if (await dc.FileModels.AnyAsync(c => c.Id == model.Id) == false)
                    return false;

                var file = await dc.FileModels.FirstOrDefaultAsync(c=> c.Id == c.Id);
                file.FileContent = model.FileContent;
                file.HashFile = model.HashFile;
                file.AssimentCode = model.AssimentCode;
                file.Key = model.Key;
                file.UpdateDate = DateTime.UtcNow;

                await dc.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> UpdateKey(Guid id, string key)
        {
            try
            {
                var dc = dbContextFactory.Create(typeof(FileRepository));

                if (await dc.FileModels.AnyAsync(c => c.Id == id) == false)
                    return false;

                var file = await dc.FileModels.FirstOrDefaultAsync(c => c.Id == c.Id);
                
                file.Key = key;
                file.UpdateDate = DateTime.UtcNow;

                await dc.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
