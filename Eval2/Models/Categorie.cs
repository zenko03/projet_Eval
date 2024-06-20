using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("categorie")]
    public class Categorie
    {
        [Key]
        [Column("idcategorie")]
        public int idcategorie {  get; set; }
        [Column("nom")]
        public string nom { get; set; }
    }
}
