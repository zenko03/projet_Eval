using Eval2.Models.Service;
using Eval2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Eval2.Data;
using Eval2.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Eval2.Models.ViewModel;

namespace Eval2.Controllers
{
    public class EtapeController : Controller
    {
        private readonly EquipeService equipeService;
        private readonly AdminService adminService;

        private readonly EtapeService etapeService;
        private readonly CoureurService coureurService;
        private readonly CoureurEtapeService coureurEtapeService;

        private readonly ResultatEtapeService resultatEtapeService;



        private readonly DataContext _datacontext;
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EtapeController(EquipeService equipeService, EtapeService etapeService, DataContext datacontext, ILogger<HomeController> logger,
            IHttpContextAccessor httpContextAccessor, CoureurService coureurService,CoureurEtapeService coureurEtapeService,AdminService adminService,
            ResultatEtapeService resultatEtapeService)
        {
            this.equipeService = equipeService;
            this.etapeService = etapeService;
            _datacontext = datacontext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            this.coureurService = coureurService;
            this.coureurEtapeService = coureurEtapeService;
            this.adminService = adminService;
            this.resultatEtapeService = resultatEtapeService;
        }


        // GET: EtapeController
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var currentUserEquipe = await equipeService.GetCurrentUserAsync();
            var currentUserAdmin = await adminService.GetCurrentUserAsync();

            if (currentUserEquipe == null && currentUserAdmin == null)
            {
                return RedirectToAction("Login", "Equipe");
            }

            ViewBag.user = currentUserEquipe;
            ViewBag.useradmin = currentUserAdmin;
            var etapes = await etapeService.GetPageAsync(pageNumber, pageSize);
            var totalPages =  etapeService.GetTotalPages(pageSize);

            var etapeViewModels = new List<EtapeViewModel>();

            foreach (var etape in etapes)
            {

                var coureurs = await coureurEtapeService.GetCoureursByEtapeIdAsync(etape.idetape);
                var coureurViewModels = new List<CoureurViewModel>();

                foreach (var coureurEtape in coureurs)
                {
                    var chronoTime = await resultatEtapeService.GetChronoTimeAsync(etape.idetape, coureurEtape.idcoureur);
                    coureurViewModels.Add(new CoureurViewModel
                    {
                        Coureur = coureurEtape.coureur,
                        ChronoTime = chronoTime
                    });
                }
                etapeViewModels.Add(new EtapeViewModel
                {
                    Etape = etape,
                    Coureurs = coureurViewModels
                });
            }

            var viewModel = new Modele<EtapeViewModel>
            {
                Data = etapeViewModels.OrderBy(e => e.Etape.rangetape),
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(viewModel);
        }

        public async Task<IActionResult> AffecterCoureur(int id)
        {
            Console.WriteLine("id via view " +id);
            var currentUser = await equipeService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Equipe");
            }

            ViewBag.user = currentUser;
            var etape = await etapeService.GetEtapeByIdAsync(id);

            var listcoureursEquipe = await coureurService.getCoureurByEquipe(currentUser.idequipe);

            foreach(var coureur in listcoureursEquipe)
            {
                Console.WriteLine("liste coureur pour equipe: "+currentUser.nom+" = "+coureur.nom);
            }

            Console.WriteLine("idetape actuel :"+etape.idetape);
            if (etape == null)
            {
                return NotFound();
            }

            // Vous pouvez également récupérer ici la liste des coureurs disponibles pour l'étape
            // et d'autres informations nécessaires pour l'affectation des coureurs.

            //ViewBag.EtapeId = etape.Id;
            ViewBag.Etape = etape;
            ViewBag.Coureurs = listcoureursEquipe;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveAffection(int EtapeId, int CoureurId)
        {
            var currentUser = await equipeService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Equipe");
            }

            ViewBag.user = currentUser;
            try
            {

                await coureurEtapeService.AddCoureurToEtapeAsync(EtapeId, CoureurId);
                ViewBag.SuccessMessage = "Coureur affecté avec succès.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            var etape = await etapeService.GetEtapeByIdAsync(EtapeId);
            var coureurs = await coureurService.getCoureurByEquipe(currentUser.idequipe);

            ViewBag.Etape = etape;
            ViewBag.Coureurs = coureurs;

            return View("AffecterCoureur");
        }

        public async Task<IActionResult> DetailEtapeAdmin(int id)
        {
            var currentUser = await adminService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Equipe");
            }

            ViewBag.useradmin = currentUser;
            var viewModel = new EtapeDetailViewModel();
            viewModel.Etape = await etapeService.GetEtapeByIdAsync(id);
            viewModel.CoureurEtapes = await coureurEtapeService.GetCoureursByEtapeIdAsync(id);
            viewModel.CoureurTempsArrive = new Dictionary<int, DateTime?>();

            foreach (var ce in viewModel.CoureurEtapes)
            {
                viewModel.CoureurTempsArrive[ce.idcoureur] = await resultatEtapeService.GetTempsArriverAsync(ce.idcoureur, id);
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTemps(int EtapeId, List<int> CoureurIds, List<TimeSpan> TempsArriver)
        {
            for (int i = 0; i < CoureurIds.Count; i++)
            {
                var resultatEtape = new ResultatEtape
                {
                    idcoureur = CoureurIds[i],
                    idetape = EtapeId,
                    tempsarriver = DateTime.Today.Add(TempsArriver[i]) // Ajouté pour éviter les erreurs de conversion TimeSpan -> DateTime
                };

                await resultatEtapeService.AddOrUpdateResultatAsync(resultatEtape);
            }

            return RedirectToAction("DetailEtapeAdmin", new { id = EtapeId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTemps(int etapeId, int coureurId)
        {
            await resultatEtapeService.DeleteResultatAsync(coureurId, etapeId);
            return RedirectToAction("DetailEtapeAdmin", new { id = etapeId });
        }

        // GET: EtapeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EtapeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EtapeController/Create
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

        // GET: EtapeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EtapeController/Edit/5
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

        // GET: EtapeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EtapeController/Delete/5
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
