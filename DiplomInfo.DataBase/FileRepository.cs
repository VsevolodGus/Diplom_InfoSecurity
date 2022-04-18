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


        public bool AddFile(FileDTO model)
        {
            files.Add(model);
            return true;
        }

        public List<FileDTO> GetFiles(string query, int skipCount, int count)
        {
            if (string.IsNullOrEmpty(query))
            {
                return files.OrderBy(c => c.DateTime).Skip(skipCount).Take(count).ToList();
            }
            else
            {
                return files.Where(x => x.Name.Contains(query))
                            .OrderBy(c => c.DateTime)
                            .Skip(skipCount).Take(count)
                            .ToList();
            }
        }


        public FileDTO GetFileById(Guid id)
        {
            return files.First(c => c.Id == id);
        }

        public bool IsExsistsFileByTitle(string name, string text = "")
        {
            return files.Any(c => c.Name == name /*&& c.Name != text*/);
        }

    }
}
