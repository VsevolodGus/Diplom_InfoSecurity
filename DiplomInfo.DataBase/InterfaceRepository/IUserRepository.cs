using DiplomInfo.DataBase.InterfaceRepository.Abstract;
using DiplomInfo.DataBase.Models;
using System;

namespace DiplomInfo.DataBase.InterfaceRepository
{
    public interface IUserRepository : IEntityRepository<UserEntity, Guid>
    {
    }
}
