using DiplomInfo.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomInfo.DataBase.InterfaceRepository
{
    public interface IFileRepository
    {
        public void Add(FileEntity model);
        public List<FileEntity> GetListBySearch(string query);
        public FileEntity GetById(Guid id);
        public bool IsExsistsByTitle(string name);
        public bool IsExsistsByTitleAndHash(string name, string hash);

    }
}
