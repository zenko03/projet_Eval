using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("categoriecoureur")]
    public class CategorieCoureur
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [ForeignKey("coureur")]
        public int idcoureur {  get; set; }
        [Column("idcoureur")]
        public Coureurs coureur { get; set; }
        [ForeignKey("categorie")]
        public int idcategorie { get; set; }
        [Column("idcategorie")]
        public Categorie categorie { get; set; }

    }
}
