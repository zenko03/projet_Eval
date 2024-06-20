using Eval2.Data;
using Eval2.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Eval2.Models.ViewModel;

namespace Eval2.Models.Service
{
    public class ClassementService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DataContext _context;

        public ClassementService(IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        //public List<ClassementViewModel> GetClassement()
        //{
        //    var classement = _context.resultat
        //        .Include(re => re.etape)
        //        .Include(re => re.coureur)
        //        .OrderBy(re => re.idetape)
        //        .ThenBy(re => re.tempsarriver)
        //        .ToList();

        //    // Dictionary pour stocker le classement final avec points
        //    var classementFinal = new List<ClassementViewModel>();

        //    // Dictionnaire pour gérer les ex-aequo
        //    var classementAvecExAequo = new Dictionary<int, List<ClassementViewModel>>(); // Dictionnaire pour stocker les ex-aequo

        //    foreach (var re in classement)
        //    {
        //        var idEtape = re.idetape;
        //        var tempsArriver = re.tempsarriver;

        //        // Récupérer le rang depuis PointEtape
        //        var pointEtape = _context.pointEtapes
        //            .FirstOrDefault(pe => pe.idetape == idEtape && pe.rang == (classementFinal.Count + 1));

        //        if (pointEtape == null)
        //        {
        //            // Si aucun point n'est trouvé pour l'étape et le rang, continuer
        //            continue;
        //        }

        //        var rang = pointEtape.rang;
        //        var point = pointEtape.point;

        //        // Créer le ViewModel correspondant
        //        var classementViewModel = new ClassementViewModel
        //        {
        //            Id = re.idresultat,
        //            IdEtape = re.idetape,
        //            IdCoureur = re.idcoureur,
        //            TempsArriver = re.tempsarriver,
        //            Rang = rang,
        //            Point = point,
        //            NomCoureur = re.coureur.nom
        //        };

        //        // Vérifier s'il y a déjà des coureurs avec ce rang (ex-aequo)
        //        if (classementAvecExAequo.ContainsKey(rang))
        //        {
        //            classementAvecExAequo[rang].Add(classementViewModel); // Ajouter au groupe d'ex-aequo
        //        }
        //        else
        //        {
        //            // Créer un nouveau groupe d'ex-aequo
        //            classementAvecExAequo[rang] = new List<ClassementViewModel> { classementViewModel };
        //        }

        //        // Ajouter au classement final
        //        classementFinal.Add(classementViewModel);
        //    }

        //    // Gérer les ex-aequo en attribuant les mêmes points à tous les coureurs ayant le même rang
        //    foreach (var rang in classementAvecExAequo.Keys)
        //    {
        //        var exAequoGroup = classementAvecExAequo[rang];
        //        var pointExAequo = exAequoGroup.First().Point;

        //        foreach (var classementViewModel in exAequoGroup)
        //        {
        //            classementViewModel.Point = pointExAequo; // Attribuer les points d'ex-aequo
        //        }
        //    }

        //    return classementFinal;
        //}


        //public List<ClassementEquipeViewModel> GetClassementEquipe()
        //{
        //    // Obtenir tous les résultats avec les informations sur les étapes et les coureurs
        //    var resultats = _context.resultat
        //        .Include(re => re.coureur)
        //        .ThenInclude(c => c.equipe)
        //        .Include(re => re.etape)
        //        .ToList();

        //    // Obtenir tous les points pour les étapes
        //    var pointsEtape = _context.pointEtapes.ToList();

        //    // Calculer les rangs et total des points par équipe
        //    var classementEquipe = resultats
        //        .GroupBy(re => re.coureur.idequipe)
        //        .Select(g =>
        //        {
        //            var totalPoints = 0;

        //            // Calculer les rangs pour chaque résultat
        //            var rangs = g.OrderBy(re => re.tempsarriver)
        //                         .Select((re, index) => new { Resultat = re, Rang = index + 1 })
        //                         .ToList();

        //            // Gérer les ex-aequo
        //            for (int i = 1; i < rangs.Count; i++)
        //            {
        //                if (rangs[i].Resultat.tempsarriver == rangs[i - 1].Resultat.tempsarriver)
        //                {
        //                    rangs[i] = new { rangs[i].Resultat, Rang = rangs[i - 1].Rang };
        //                }
        //            }

        //            // Calculer les points totaux pour l'équipe
        //            foreach (var rang in rangs)
        //            {
        //                var pointEtape = pointsEtape.FirstOrDefault(pe => pe.idetape == rang.Resultat.idetape && pe.rang == rang.Rang);
        //                totalPoints += pointEtape?.point ?? 0;
        //            }

        //            return new ClassementEquipeViewModel
        //            {
        //                IdEquipe = g.Key,
        //                NomEquipe = g.First().coureur.equipe.nom,
        //                TotalPoints = totalPoints
        //            };
        //        })
        //        .OrderByDescending(ce => ce.TotalPoints)
        //        .ToList();

        //    return classementEquipe;
        //}

        public async Task CalculateEquipePoints()
        {
            // Effacer les points précédents
            var allEquipePoints = await _context.equipePoints.ToListAsync();
            _context.equipePoints.RemoveRange(allEquipePoints);
            await _context.SaveChangesAsync();

            foreach(var pointequip in allEquipePoints)
            {
                Console.WriteLine("pointequiipe "+pointequip.idequipe);
            }
            // Utiliser la vue pour obtenir les points des coureurs
            var coureurPoints = await _context.coureurPoints.ToListAsync();

            foreach (var coureurpoint in coureurPoints)
            {
                Console.WriteLine("point coureurs"+ coureurpoint.points);
            }
            var equipePoints = coureurPoints
                .GroupBy(cp => cp.idequipe)
                .Select(g => new EquipePoints
                {
                    idequipe = g.Key,
                    totalpoint = g.Sum(cp => cp.points)
                }).ToList();

            // Sauvegarder les points par équipe
            _context.equipePoints.AddRange(equipePoints);
            await _context.SaveChangesAsync();
        }

        public async Task<List<V_classementequipe>> GetEquipeClassement()
        {
            var result = await _context.classementsEquipe.ToListAsync();

            if (!result.Any())
            {
                throw new InvalidOperationException("No data found in ClassementEquipe view.");
            }

            return result;
        }

        public async Task<List<V_classement>> GetClassementParGenre(string genre)
        {
            return await _context.classements
                .Where(c => c.genrecoureur == genre)
                .ToListAsync();
        }


        public async Task CalculateEquipePointsByGenre(string genre)
        {

            Console.WriteLine("genre "+genre);
            // Effacer les points précédents pour ce genre
            var allEquipePoints = await _context.equipePointsGenre.Where(ep => ep.genre == genre).ToListAsync();
            _context.equipePointsGenre.RemoveRange(allEquipePoints);
            await _context.SaveChangesAsync();

            foreach ( var equipe in allEquipePoints)
            {
                Console.WriteLine("genre equipe :"+equipe.genre+" = "+genre);
            }

            // Utiliser la vue pour obtenir les points des coureurs par genre
            var coureurPointsGenre = await _context.coureurPointsGenre.Where(cp => cp.genre == genre).ToListAsync();
            var equipePointsGenre = coureurPointsGenre
                .GroupBy(cp => cp.idequipe)
                .Select(g => new EquipesPointsGenre
                {
                    idequipe = g.Key,
                    genre = genre,
                    totalpoint = g.Sum(cp => cp.points)
                }).ToList();

            // Sauvegarder les points par équipe pour ce genre
            _context.equipePointsGenre.AddRange(equipePointsGenre);
            await _context.SaveChangesAsync();
        }


        public async Task<List<EquipesPointsGenre>> GetEquipeClassementByGenre(string genre)
        {
            return await _context.equipePointsGenre.Where(ep => ep.genre == genre).ToListAsync();
        }


        public async Task<List<ClassementGeneraleEquipe>> GetClassementEquipes( int categorieId, string order, string triColumn)
        {
            var classementParCategorie = _context.classementsCategorie
                .Where(v => v.idcategorie == categorieId)
                .Select(v => new
                {
                    v.idequipe,
                    v.nomequipe,
                    v.idetape,
                    v.datearrivee
                })
                .ToList();

            var rankedResultsWithPoints = classementParCategorie
                .GroupBy(v => new { v.idequipe, v.nomequipe })
                .Select(g => new ClassementGeneraleEquipe
                {
                    EquipeId = g.Key.idequipe,
                    NomEquipe = g.Key.nomequipe,
                    Points = g.Count(), // Assurez-vous de la logique pour les points, ici c'est juste un exemple
                })
                .OrderByDescending(c => c.Points)
                .ToList();

            var classements = rankedResultsWithPoints
                .Select((c, index) => new ClassementGeneraleEquipe
                {
                    EquipeId = c.EquipeId,
                    NomEquipe = c.NomEquipe,
                    Points = c.Points,
                    Rang = index + 1
                })
                .ToList();

            // Optional: Sorting logic based on order and triColumn parameters
            if (!string.IsNullOrEmpty(triColumn) && !string.IsNullOrEmpty(order))
            {
                switch (triColumn.ToLower())
                {
                    case "nom":
                        classements = order.ToLower() == "desc" ?
                            classements.OrderByDescending(c => c.NomEquipe).ToList() :
                            classements.OrderBy(c => c.NomEquipe).ToList();
                        break;
                    case "points":
                        classements = order.ToLower() == "desc" ?
                            classements.OrderByDescending(c => c.Points).ToList() :
                            classements.OrderBy(c => c.Points).ToList();
                        break;
                    case "rang":
                        classements = order.ToLower() == "desc" ?
                            classements.OrderByDescending(c => c.Rang).ToList() :
                            classements.OrderBy(c => c.Rang).ToList();
                        break;
                    default:
                        break;
                }
            }

            return await Task.FromResult(classements);
        }




    }
}
