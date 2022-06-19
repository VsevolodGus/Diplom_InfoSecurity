using System;
using System.Linq;
using System.Collections.Generic;
using DiplomInfo.DataBase.Models;
using DiplomInfo.DataBase.InterfaceRepository;

namespace DiplomInfo.DataBase
{
    internal class FileRepository : IFileRepository  
    {
        private readonly List<FileEntity> files;

        public FileRepository()
        {
            files = new List<FileEntity>();
        }


        public void Add(FileEntity model)
        {
            files.Add(model);
        }

        public List<FileEntity> GetListBySearch(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return files.ToList();
            }
            else
            {
                return files.Where(x => x.Name.Contains(query))
                            .ToList();
            }
        }


        public FileEntity GetById(Guid id)
        {
            return files.First(c => c.Id == id);
        }

        public bool IsExsistsByTitle(string name)
        {
            return files.Any(c => c.Name == name);
        }

        public bool IsExsistsByTitleAndHash(string name, string hash)
        {
            return files.Any(c => c.Name == name && c.Hash != hash);
        }

    }
}
