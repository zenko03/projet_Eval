using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("etape")]
    public class Etape
    {
        [Key]
        [Column("idetape")]
        public int idetape {  get; set; }
        [Column("nom")]
        public string? nom { get; set; }
        [Column("distance")]
        public double? distance { get; set; }
        [Column("nbrcoureur")]
        public int? nbrcoureur { get; set; }
        [Column("rangetape")]
        public int? rangetape { get; set; }
       
        [ForeignKey("course")]
        public int? idcourse { get; set; }
        [Column("idcourse")]
        public Course? course { get; set; }
       
        [Column("tempsdepart")]
        public DateTime? tempsdepart {  get; set; }
    }
}
