using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("coureur")]
    public class Coureurs
    {
        [Key]
        [Column("idcoureur")]
        public int idcoureur {  get; set; }
        [Column("nom")]
        public string nom { get; set; }
        [Column("numdossard")]
        public int numdossard { get; set; }
        [Column("genre")]
        public string genre { get; set; }
        [Column("ddn")]
        public DateTime ddn { get; set; }
  
        [ForeignKey("equipe")]
        public int idequipe { get; set; }
        [Column("equipe")]
        public Equipe equipe { get; set; }

    }
}
