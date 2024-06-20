using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("classement")]
    public class ClassementEtape
    {
        [Key]
        [Column("idclassement")]
        public int idclassement {  get; set; }
        [ForeignKey("etape")]
        public int idetape { get; set; }
        [Column("idetape")]
        public Etape etape { get; set; }
        [ForeignKey("coureur")]
        public int idcoureur { get; set; }
        [Column("idcoureur")]
        public Coureurs coureur {  get; set; }
        [Column("point")]
        public int? point { get; set; }
        public ClassementEtape() { }

    }
}
