using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomInfo.DataBase.Models
{
    public class FileDTO
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public string Blob { get; set; }

        public string Hash { get; set; }

        public string AsymetricCode { get; set; }

        public string KeyEncrypt { get; init; }

        public string KeyDecrypt { get; init; }
    }
}
