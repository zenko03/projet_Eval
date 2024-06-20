using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("v_resultatetapeclassement")]
    public class V_ResultatEtapeClassement
    {
        public int idetape {  get; set; }
        public int idcoureur { get; set; }
        public DateTime tempsarriver { get; set; }
        public int idequipe { get; set; }
        public int rang {  get; set; }
    }
}
