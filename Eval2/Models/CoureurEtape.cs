using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("coureuretape")]
    
    public class CoureurEtape
    {
        [Key]
        [Column("id")]
        public int id {  get; set; }
        [ForeignKey("coureur")]
        public int idcoureur {  get; set; }
        [Column("idcoureur")]
        public Coureurs coureur { get; set; }
        [ForeignKey("etape")]
        public int idetape { get; set; }
        [Column("idetape")]
        public Etape etape {  get; set; }
    }
}
