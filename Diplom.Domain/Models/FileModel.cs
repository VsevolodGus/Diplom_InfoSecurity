using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Domain.Models
{
    public class FileModel
    {
        public FileModel(Guid fileId, string Name, string text)
        {

        }
        public Guid Id { get; init; }

        public string Name { get; init; }

        public string Text { get; set; }

        public string Hash { get; set; }

        public string AsymetricCode { get; set; }

        public string KeyEncrypt { get; set; }

        public string KeyDecrypt { get; set; }

        public DateTime DateTime { get; init; }

        public DateTime Date { get => DateTime.Date; }

        public TimeSpan Time { get => DateTime.TimeOfDay; }

    }
}
