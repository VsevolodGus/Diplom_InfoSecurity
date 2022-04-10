using System;

namespace DiplomInfo.DataBase.Models
{
    public class User
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; }
        public string SecondName { get; init; }
        public string ThirdName { get; init; }
    }
}
