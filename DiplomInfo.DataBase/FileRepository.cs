using DiplomInfo.DataBase.Models;
using System.Linq;
using System.Collections.Generic;
using System;

namespace DiplomInfo.DataBase
{
    

    public class FileRepository
    {
        private readonly List<FileEntity> files;

        public FileRepository()
        {
            this.files = new List<FileEntity>()
            {
                new FileEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "File1",
                    Text = "11111",
                    AsymetricCode = "asd",
                    Hash = "dsa",
                    KeyDecrypt = "zxc",
                    KeyEncrypt = "cxz",
                    DateTime = DateTime.Now,
                },
                new FileEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "File2",
                    Text = "22222",AsymetricCode = "asd",
                    Hash = "dsa",
                    KeyDecrypt = "zxc",
                    KeyEncrypt = "cxz",
                    DateTime = DateTime.Now,
                },
                new FileEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "File3",
                    Text = "33333",
                    AsymetricCode = "asd",
                    Hash = "dsa",
                    KeyDecrypt = "zxc",
                    KeyEncrypt = "cxz",
                    DateTime = DateTime.Now,
                }
            };
        }


<<<<<<< Updated upstream
        public bool AddFile(FileDTO model)
=======
        public bool Add(FileEntity model)
>>>>>>> Stashed changes
        {
            files.Add(model);
            // dc.SaveChanges();
            return true;
        }

        public List<FileEntity> GetFiles(string query, int skipCount, int count)
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


        public FileEntity GetFileById(Guid id)
        {
            var model = files.First(c => c.Id == id);
            return model;
        }



    }
}
