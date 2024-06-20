using Eval2.Data;
using Eval2.Models.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eval2.Controllers
{
    public class ClassementController : Controller
    {
        private readonly EquipeService equipeService;
        private readonly AdminService adminService;
        private readonly ClassementService classementService;


        private readonly DataContext _datacontext;
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClassementController(EquipeService equipeService, AdminService adminService, ClassementService classementService, DataContext datacontext, ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.equipeService = equipeService;
            this.adminService = adminService;
            this.classementService = classementService;
            _datacontext = datacontext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }


        // GET: ClassementController
        public ActionResult Index()
        {
            return View();
        }
        //public async Task<IActionResult> ClassementGeneral()
        //{

        //    var currentUserEquipe = await equipeService.GetCurrentUserAsync();
        //    var currentUserAdmin = await adminService.GetCurrentUserAsync();

        //    if (currentUserEquipe == null && currentUserAdmin == null)
        //    {
        //        return RedirectToAction("Login", "Equipe");
        //    }

        //    ViewBag.user = currentUserEquipe;
        //    ViewBag.useradmin = currentUserAdmin;
        //    var classementGeneral = classementService.GetClassement();
        //    return View(classementGeneral);
        //}
        //public async Task<IActionResult> ClassementEquipe()
        //{
        //    var currentUserEquipe = await equipeService.GetCurrentUserAsync();
        //    var currentUserAdmin = await adminService.GetCurrentUserAsync();

        //    if (currentUserEquipe == null || currentUserAdmin == null)
        //    {
        //        return RedirectToAction("Login", "Equipe");
        //    }

        //    ViewBag.user = currentUserEquipe;
        //    ViewBag.useradmin = currentUserAdmin;
        //    var classementEquipe = classementService.GetClassementEquipe();
        //    return View(classementEquipe);
        //}

        public async Task<IActionResult> CalculerClassement()
        {
             await classementService.CalculateEquipePoints();
            TempData["Message"] = "Classement général calculé avec succès.";
            return RedirectToAction("ClassementGeneral");
        }

        public async Task<IActionResult> ClassementGeneral()
        {
            var currentUserEquipe = await equipeService.GetCurrentUserAsync();
            var currentUserAdmin = await adminService.GetCurrentUserAsync();

            if (currentUserEquipe == null && currentUserAdmin == null)
            {
                return RedirectToAction("Login", "Equipe");
            }

          //  var view = await _datacontext.resultatEtapeClassements.CountAsync();

            ViewBag.user = currentUserEquipe;
            ViewBag.useradmin = currentUserAdmin;

            var classement = await classementService.GetEquipeClassement();
            return View(classement);
        }


        public async Task<IActionResult> CalculerClassementParGenre(string genre)
        {
            await classementService.CalculateEquipePointsByGenre(genre);
            TempData["Message"] = $"Classement général pour la catégorie {genre} calculé avec succès.";
            return RedirectToAction("ClassementParGenre", new { genre });
        }

        //public async Task<IActionResult> ClassementParGenre(string genre)
        //{
        //    var currentUserEquipe = await equipeService.GetCurrentUserAsync();
        //    var currentUserAdmin = await adminService.GetCurrentUserAsync();

        //    if (currentUserEquipe == null && currentUserAdmin == null)
        //    {
        //        return RedirectToAction("Login", "Equipe");
        //    }

        //    ViewBag.User = currentUserEquipe;
        //    ViewBag.UserAdmin = currentUserAdmin;
        //    var classement = await classementService.GetClassementParGenre(genre);
        //    return View(classement);
        //}



        // GET: ClassementController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClassementController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClassementController/Create
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
        public async Task<IActionResult> ClassementParGenre(int categorieId, string order , string triColumn)
        {
            var currentUserEquipe = await equipeService.GetCurrentUserAsync();
            var currentUserAdmin = await adminService.GetCurrentUserAsync();

            if (currentUserEquipe == null && currentUserAdmin == null)
            {
                return RedirectToAction("Login", "Equipe");
            }
                ViewBag.User = currentUserEquipe;
                ViewBag.UserAdmin = currentUserAdmin;
            var classements = await classementService.GetClassementEquipes(categorieId, order, triColumn);
            return View(classements);
        }

        
        // GET: ClassementController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClassementController/Edit/5
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

        // GET: ClassementController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClassementController/Delete/5
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
