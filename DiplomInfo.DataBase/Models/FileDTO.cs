using System;

namespace DiplomInfo.DataBase.Models
{
    public class FileDTO
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public string Text { get; set; }

        public string Hash { get; set; }

        public string AsymetricCode { get; set; }

        public string KeyEncrypt { get; set; }

        public string KeyDecrypt { get; set; }

        public DateTime DateTime { get; init; }

        public byte[] EncBytes { get; set; }
    }
}
