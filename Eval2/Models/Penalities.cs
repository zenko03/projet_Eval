using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("penalities")]
    public class Penalities
    {
        [Key]
        [Column("idpenality")]
        public int idpenality {  get; set; }
        [ForeignKey("etape")]
        public int idetape { get; set; }
        [Column("idetape")]     
        public Etape etape { get; set; }
        [ForeignKey("equipe")]
        public int idequipe { get; set; }

        [Column("idequipe")]
        public Equipe equipe {  get; set; }
        [Column("motif")]
        public string? motif {  get; set; }
        [Column("tempspenalite",TypeName ="Time" )]
        public string tempspenalite { get; set; }
        [Column("datepenalite")]

        public DateTime? datepenalite {  get; set; }

        public Penalities() { }
    }
}
