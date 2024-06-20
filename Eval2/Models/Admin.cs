using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("admin")]
    public class Admin
    {
        [Key]
        [Column("idadmin")]
        public int idadmin { get; set; }
        [Column("email")]
        public string email { get; set; }
        [Column("nom")]
        public string? nom {  get; set; }
        [Column("mdp")]
        [MaxLength(255)]
        public string mdp { get; set; }
        [Column("etat")]
        public int? etat { get; set; }

        public Admin() {  }

    }
}
