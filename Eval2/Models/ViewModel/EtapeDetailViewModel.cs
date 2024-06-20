namespace Eval2.Models.ViewModel
{
    public class EtapeDetailViewModel
    {
        public Etape Etape { get; set; }
        public List<CoureurEtape> CoureurEtapes { get; set; }
        public Dictionary<int, DateTime?> CoureurTempsArrive { get; set; }

        public EtapeDetailViewModel() { }
    }
}
