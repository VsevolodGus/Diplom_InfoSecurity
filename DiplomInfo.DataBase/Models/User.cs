using System;

namespace DiplomInfo.DataBase.Models
{
    public class UserEntity
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public string Patronymic { get; init; }
    }
}
