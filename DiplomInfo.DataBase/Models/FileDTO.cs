using System;

namespace DiplomInfo.DataBase.Models
{
    public class FileEntity
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public string Hash { get; set; }

        public string AsymetricCode { get; set; }

        public DateTime DateTime { get; init; }

        public byte[] EncBytes { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
