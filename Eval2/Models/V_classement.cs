using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("v_classement")]
    public class V_classement
    {
        public int etapeid { get; set; }
        public string nometape { get; set; }
        public double longueur { get; set; }
        public int nbcoureur { get; set; }
        public DateTime datedepart { get; set; }
        public int rangetape { get; set; }
        public int coureurid { get; set; }
        public string nomcoureur { get; set; }
        public string genrecoureur { get; set; }
        public int numdossard { get; set; }
        public DateTime datearrivee { get; set; }
        public int idequipe { get; set; }
        public string nomequipe { get; set; }
        public int rang { get; set; }
        public int points { get; set; }
    }
}
