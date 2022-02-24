using System;
using System.ComponentModel.DataAnnotations;

namespace Diplom.DataBase.Models
{
    public class FileModel
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        public string FileContent { get; set; }

        [Required]
        public string HashFile { get; set; }

        [Required]
        public string AssimentCode { get; set; }
        
        [Required]
        public string Key { get; set; }

        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
