using Diplom.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Domain.Models
{
    public class FileModel
    {
        public FileModel(Guid fileId, string name, string text, DateTime dateTime)
        {
            this.Id = fileId;
            this.Name = name;
            this.Text = text;
            this.DateTime = dateTime;
            this.Hash = SecurityUtils.CalculateMD5Hash(text);
            this.EncrypText = SecurityUtils.Encrypt(text, out byte[] encBytes, out string encKey);
        }
        public Guid Id { get; init; }

        public string Name { get; init; }

        public string Text { get; set; }

        public string Hash { get; set; }

        public string EncrypText{ get; set; }

        //public string KeyEncrypt { get; set; }

        //public string KeyDecrypt { get; set; }

        private DateTime DateTime;

        public DateTime Date { get => DateTime.Date; }

        public TimeSpan Time { get => DateTime.TimeOfDay; }

    }
}
