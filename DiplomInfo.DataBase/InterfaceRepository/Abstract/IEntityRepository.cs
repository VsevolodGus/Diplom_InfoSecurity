using System.Collections.Generic;

namespace DiplomInfo.DataBase.InterfaceRepository.Abstract
{
    public interface IEntityRepository<T, TypeId>
    {
        public void Add(T model);
        public List<T> GetList(string query);
        public T GetById(TypeId id);
        public bool IsExsistsById(TypeId id);
    }
}
