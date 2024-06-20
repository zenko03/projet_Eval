using Eval2.Data;
using Eval2.Models;
using Eval2.Models.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eval2.Controllers
{
    public class CategorieController : Controller
    {
        private readonly EquipeService equipeService;
        private readonly AdminService adminService;
        private readonly CategorieCoureurService categorieCoureurService;

        private readonly DataContext _datacontext;
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategorieController(EquipeService equipeService, AdminService adminService, DataContext datacontext,
            ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, CategorieCoureurService categorieCoureurService)
        {
            this.equipeService = equipeService;
            this.adminService = adminService;
            _datacontext = datacontext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            this.categorieCoureurService = categorieCoureurService;
        }
        private int CalculerAge(DateTime dateDeNaissance)
        {
            var today = DateTime.Today;
            var age = today.Year - dateDeNaissance.Year;
            if (dateDeNaissance.Date > today.AddYears(-age)) age--;
            return age;
        }

        public async Task<IActionResult> GenererCategories()
        {
            var coureurs = await _datacontext.coureur.ToListAsync();
            var categories = await _datacontext.categorie.ToListAsync();
            bool categoriesGenerated = false;

            foreach (var coureur in coureurs)
            {
                List<Categorie> categoriePossible = new List<Categorie>();
                Dictionary<string, List<string>> sexeKey = categorieCoureurService.GetPossibleSexeKey();

                foreach (var categorie in categories)
                {
                    if (sexeKey.ContainsKey(categorie.nom))
                    {
                        List<string> genre = sexeKey[categorie.nom];
                        foreach (string g in genre)
                        {
                            if (g.Equals(coureur.genre))
                            {
                                categoriePossible.Add(categorie);
                            }
                        }
                    }

                    if (categorie.nom.Equals("Junior") && categorieCoureurService.IsJunior(coureur.ddn))
                    {
                        categoriePossible.Add(categorie);
                    }
                }

                foreach (var categorie in categoriePossible)
                {
                    if (!_datacontext.categorieCoureurs.Any(ca => ca.idcategorie == categorie.idcategorie && ca.idcoureur == coureur.idcoureur))
                    {
                        var categoriecoureur = new CategorieCoureur
                        {
                            idcategorie = categorie.idcategorie,
                            idcoureur = coureur.idcoureur
                        };
                        _datacontext.categorieCoureurs.Add(categoriecoureur);
                        categoriesGenerated = true;

                    }
                }
            }
            await _datacontext.SaveChangesAsync();
            if (categoriesGenerated)
            {
                TempData["Message"] = "Catégories générées avec succès.";
            }
            else
            {
                TempData["Message"] = "Génération des catégories déjà attribuée pour chaque coureur.";
            }
            return RedirectToAction("Accueil", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> ReinitialiserCategories()
        {
            var categories = await _datacontext.categorieCoureurs.ToListAsync();
            if (categories.Count > 0)
            {
                _datacontext.categorieCoureurs.RemoveRange(categories);
                await _datacontext.SaveChangesAsync();
                TempData["Message"] = "Catégories réinitialisées avec succès.";
            }
            else
            {
                TempData["Message"] = "Aucune catégorie à réinitialiser.";
            }

            return RedirectToAction("Accueil", "Admin");
        }

        // GET: CategorieController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CategorieController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategorieController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategorieController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategorieController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategorieController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategorieController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategorieController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
