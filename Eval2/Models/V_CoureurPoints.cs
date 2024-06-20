using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("v_coureurpoints")]
    public class V_CoureurPoints
    {
        public int idetape {  get; set; }
        public int idcoureur { get; set; }
        public int idequipe { get; set; }
        [Column("points")]
        public int points {  get; set; }
    }
}
