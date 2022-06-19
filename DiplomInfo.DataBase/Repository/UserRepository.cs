using System;
using System.Linq;
using System.Collections.Generic;
using DiplomInfo.DataBase.Models;
using DiplomInfo.DataBase.InterfaceRepository;

namespace DiplomInfo.DataBase.Repository
{
    public class UserRepository : IUserRepository  
    {
        private readonly List<UserEntity> users;

        public UserRepository()
        {
            users = new List<UserEntity>();
        }

        public void Add(UserEntity model)
        {
            users.Add(model);
        }

        public List<UserEntity> GetList(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return users.ToList();
            }
            else
            {
                return users.Where(x => x.Name.Contains(query))
                            .ToList();
            }
        }


        public UserEntity GetById(Guid id)
        {
            return users.First(c => c.Id == id);
        }

        public bool IsExsistsById(Guid id)
        {
            return users.Any(c => c.Id == id);
        }
    }
}
