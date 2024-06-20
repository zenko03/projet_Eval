using Eval2.Data;
using Eval2.Models.Import;
using Eval2.Models.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Eval2.Controllers
{
    public class ImportController : Controller
    {
        private readonly EquipeService equipeService;
        private readonly AdminService adminService;
        private readonly ClassementService classementService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly DataContext dataContext;





        private readonly DataContext _datacontext;
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImportController(EquipeService equipeService, AdminService adminService, ClassementService classementService,
            DataContext datacontext, ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostEnvironment
            , DataContext dataContext)
        {
            this.equipeService = equipeService;
            this.adminService = adminService;
            this.classementService = classementService;
            _datacontext = datacontext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _hostEnvironment = hostEnvironment;
            this.dataContext = dataContext;
        }



        // GET: ImportController
        public ActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ImportDonnee()
        {
            var currentUser = await adminService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            ViewBag.useradmin = currentUser;
            return View();
        }
        public async Task<IActionResult> ImportDonneePoint()
        {
            var currentUser = await adminService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.useradmin = currentUser;

            return View();
        }

        public async Task<IActionResult> importer( IFormFile fichier2)
        {
            var currentUser = await adminService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            //CsvEtapes csvetape = new CsvEtapes();
            //if (fichier1 != null && fichier1.Length > 0)
            //{
            //    var result1 = await csvetape.DispatchToTableAsync(dataContext, _hostEnvironment, "csv", fichier1);
            //    if (result1.LineErrors.Any())
            //    {
            //        foreach (var erreur in result1.LineErrors)
            //        {
            //            ModelState.AddModelError("", $"Erreur dans le fichier 1 à la ligne {erreur.Line}: {erreur.Error}");
            //        }
            //    }
            //}
            CsvResultat csvResultat = new CsvResultat();
            if(fichier2 != null && fichier2.Length > 0)
            {
                var result2 = await csvResultat.DispatchToTableAsync(dataContext, _hostEnvironment, "csv", fichier2);
                if (result2.LineErrors.Any())
                {
                    foreach (var erreur in result2.LineErrors)
                    {
                        ModelState.AddModelError("", $"Erreur dans le fichier 1 à la ligne {erreur.Line}: {erreur.Error}");
                    }
                }
            }

            ViewBag.useradmin = currentUser;
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Importation réussie.";
            }
            else
            {
                ViewBag.Message = "Des erreurs sont survenues pendant l'importation.";
            }
            return RedirectToAction("ImportDonnee");

        }
        public async Task<IActionResult> importerPoint(IFormFile fichier)
        {
            var currentUser = await adminService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            CsvPoints csvPoints = new CsvPoints();
            if (fichier != null && fichier.Length > 0)
            {
                var result2 = await csvPoints.DispatchToTableAsync(dataContext, _hostEnvironment, "csv", fichier);
                if (result2.LineErrors.Any())
                {
                    foreach (var erreur in result2.LineErrors)
                    {
                        ModelState.AddModelError("", $"Erreur dans le fichier 1 à la ligne {erreur.Line}: {erreur.Error}");
                    }
                }
            }
            ViewBag.useradmin = currentUser;
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Importation réussie.";
            }
            else
            {
                ViewBag.Message = "Des erreurs sont survenues pendant l'importation.";
            }
            return RedirectToAction("ImportDonneePoint");
        }

        // GET: ImportController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ImportController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImportController/Create
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

        // GET: ImportController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ImportController/Edit/5
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

        // GET: ImportController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ImportController/Delete/5
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
