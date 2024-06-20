using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("v_coureurpoints_genre")]
    public class V_CoureurPoints_Genre
    {
        public int idetape { get; set; }
        public int idcoureur { get; set; }
        public string genre { get; set; }
        public int idequipe { get; set; }
        [Column("points")]
        public int points { get; set; }
    }
}
