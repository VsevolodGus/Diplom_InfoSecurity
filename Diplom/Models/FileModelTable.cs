using DiplomInfo.DataBase.Models;
using System;

namespace Diplom.Models
{
    public class FileModelTable
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime DateTme { get; set; }

        public User User { get; set; } = new User()
        {
            Id = Guid.NewGuid(),
            FirstName = "",
            SecondName = "",
            ThirdName= ""
        };
    }
}
