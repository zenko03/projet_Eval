namespace Eval2.Models.ViewModel
{
    public class ClassementViewModel
    {
        public int Id { get; set; }
        public int IdEtape { get; set; }
        public int IdCoureur { get; set; }
        public DateTime TempsArriver { get; set; }
        public int Rang { get; set; }
        public int Point { get; set; }
        public string NomCoureur { get; set; }
    }
}
