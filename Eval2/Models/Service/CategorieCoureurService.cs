using Eval2.Data;

namespace Eval2.Models.Service
{
    public class CategorieCoureurService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DataContext _context;

        public CategorieCoureurService(IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public  int CalculerAge(DateTime dateDeNaissance)
        {
            var today = DateTime.Today;
            var age = today.Year - dateDeNaissance.Year;
            if (dateDeNaissance.Date > today.AddYears(-age)) age--;
            return age;
        }

        public Dictionary<string, List<string>> GetPossibleSexeKey()
        {
            Dictionary<string, List<string>> sexeKey = new Dictionary<string, List<string>>();
            sexeKey.Add("Homme", new List<string>
            {
                "H",
                "M",
                "L",
                "Homme",
                "Male",
                "Masc"
            });
            sexeKey.Add("Femme", new List<string>
            {
                "F",
                "V",
                "Femme",
                "Femelle",
                "Fem"
            });
            return sexeKey;
        }

        public bool IsJunior(DateTime dateNaissance)
        {
            var today = DateTime.Today;
            var birthDate = new DateTime(dateNaissance.Year, dateNaissance.Month, dateNaissance.Day);
            var age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age)) age--;

            return age < 18;
        }

    }
}
