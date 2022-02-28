using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Diplom.DataBase.Models
{
    public class GroupByKey
    {
        [Key]
        public long PKID { get; init; }

        [Required]
        public string Key { get; set; }

        public ICollection<FileModel> FileModels { get; init; }
    }
}
