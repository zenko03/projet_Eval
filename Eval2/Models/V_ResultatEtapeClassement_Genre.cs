using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("v_resultatetapeclassement_genre ")]
    public class V_ResultatEtapeClassement_Genre
    {
        public int idetape {  get; set; }
        public int idcoureur { get; set; }
        public DateTime tempsarriver { get; set; }
        public string genre { get; set; }
        public int idequipe {  get; set; }
        public int rang {  get; set; }
    }
}
