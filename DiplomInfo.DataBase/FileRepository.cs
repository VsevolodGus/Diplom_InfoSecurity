using System;
using System.Linq;
using System.Collections.Generic;
using DiplomInfo.DataBase.Models;


namespace DiplomInfo.DataBase
{
    public class FileRepository
    {
        private readonly List<FileDTO> files;

        public FileRepository()
        {
            this.files = new List<FileDTO>();
        }


        public void AddFile(FileDTO model)
        {
            files.Add(model);
        }

        public List<FileDTO> GetFiles(string query)
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


        public FileDTO GetFileById(Guid id)
        {
            return files.First(c => c.Id == id);
        }

        public bool IsExsistsFileByTitle(string name)
        {
            return files.Any(c => c.Name == name);
        }

        public bool IsExsistsFileByTitleAndHash(string name, string hash)
        {
            return files.Any(c => c.Name == name && c.Hash != hash);
        }

    }
}
