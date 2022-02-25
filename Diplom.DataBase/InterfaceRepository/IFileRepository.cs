using System.Threading.Tasks;
using Diplom.DataBase.Models;

namespace Diplom.DataBase.InterfaceRepository
{
    public interface IFileRepository
    {
        Task<bool> AddFile(FileModel model);
    }
}
