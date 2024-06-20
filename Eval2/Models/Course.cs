using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("course")]
    public class Course
    {
        [Key]
        [Column("idcourse")]
        public int idcourse {  get; set; }
        [Column("nom")]
        public string? nom { get; set; }
        [Column("numerocourse")]
        [MaxLength(255)]
        public string? numerocourse {  get; set; }
        
        public DateTime datecourse {  get; set; }
        
        public Course() { }
       
    }
}
