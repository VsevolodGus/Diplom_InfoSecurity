using DiplomInfo.DataBase.Models;
using System.Linq;
using System.Collections.Generic;
using System;

namespace DiplomInfo.DataBase
{
    

    public class FileRepository
    {
        private readonly List<FileDTO> files;

        public FileRepository()
        {
            this.files = new List<FileDTO>()
            {
                new FileDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "File1",
                    Blob = "11111",
                    AsymetricCode = "asd",
                    Hash = "dsa",
                    KeyDecrypt = "zxc",
                    KeyEncrypt = "cxz"
                },
                new FileDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "File2",
                    Blob = "22222",AsymetricCode = "asd",
                    Hash = "dsa",
                    KeyDecrypt = "zxc",
                    KeyEncrypt = "cxz",

                },
                new FileDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "File3",
                    Blob = "33333",
                    AsymetricCode = "asd",
                    Hash = "dsa",
                    KeyDecrypt = "zxc",
                    KeyEncrypt = "cxz",
                }
            };
        }


        public bool AddFile(FileDTO model)
        {
            files.Add(model);
            // dc.SaveChanges();
            return true;
        }

        public List<FileDTO> GetFiles(string query, int skipCount, int count)
        {
            if (string.IsNullOrEmpty(query))
            {
                return files.OrderBy(c => c.Id).Skip(skipCount).Take(count).ToList();
            }
            else
            {
                return files.Where(x => x.Name.Contains(query))
                            .OrderBy(c => c.Id)
                            .Skip(skipCount).Take(count)
                            .ToList();
            }
        }


        public FileDTO GetFileById(Guid id)
        {
            var model = files.First(c => c.Id == id);
            return model;
        }



    }
}
