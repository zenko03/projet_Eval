using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("pointetape")]
    public class PointEtape
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
       
        [Column("point")]
        public int point {  get; set; }
        [Column("rang")]
        public int rang {  get; set; }
       
        public PointEtape() { }
    }
}
