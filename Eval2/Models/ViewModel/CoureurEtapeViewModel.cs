namespace Eval2.Models.ViewModel
{
    public class CoureurEtapeViewModel
    {
        public int idEtape { get; set; }
        public int idCoureur { get; set; }
        public string CoureurNom { get; set; }
        public string CoureurEquipe { get; set; }
        public DateTime? TempsArriver { get; set; }
    }
}
