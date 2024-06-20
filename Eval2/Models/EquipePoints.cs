using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    public class EquipePoints
    {
        [Key]
        [Column("id")]
        public int id {  get; set; }
        [Column("totalpoints")]
        public int totalpoint { get; set; }
        [ForeignKey("equipe")]     
        public int idequipe {  get; set; }
        [Column("idequipe")]
        public Equipe equipe { get; set; }
    }
}
