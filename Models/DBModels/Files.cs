using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagment.Model
{
    [Table(name: "TblFiles")]
    public class Files
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string FileName { get; set; }
        
        [Column(TypeName = "varbinary(MAX)")]
        [Required]
        public Byte[] FileData { get; set; }
       
        [ForeignKey("Folders")]
        public int FoldersID { get; set; }
    }
}
