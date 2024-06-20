using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Eval2.Models
{
    [Table("equipespointsgenre")]
    public class EquipesPointsGenre
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("totalpoints")]
        public int? totalpoint { get; set; }
        [ForeignKey("equipe")]
        [Column("idequipe")]
        public int idequipe { get; set; }
        public Equipe equipe { get; set; }
        [Column("genre")]
        public string genre { get; set; }
    }
}
