using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagment.Model
{
    [Table(name:"TblFolders")]
    public class Folders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string FolderName { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [Required]
        public string FolderPath { get; set; }
      
        //public int ParentID { get; set; }

        public virtual ICollection<Files> file { get; set; }

    }
}
