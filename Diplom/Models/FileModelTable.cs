using DiplomInfo.DataBase.Models;
using System;

namespace Diplom.Models
{
    public class FileModelTable
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime DateTme { get; set; }

        public UserEntity User { get; set; } = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = "",
            Surname = "",
            Patronymic= ""
        };
    }
}
