using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Eval2.Models
{
    [Table("resultat")]
    public class ResultatEtape
    {
        [Key]
        [Column("idresultat")]
        public int idresultat {  get; set; }
       
        [Column("tempsarriver")]
        public DateTime tempsarriver{ get; set; }

       
        [ForeignKey("coureur")]
        public int idcoureur { get; set; }
        [Column("idcoureur")]
        public Coureurs coureur { get; set; }
        [ForeignKey("etape")]
        public int idetape { get; set; }
        [Column("idetape")]
        public Etape etape { get; set; }
        public ResultatEtape() { }
        
    }
}
