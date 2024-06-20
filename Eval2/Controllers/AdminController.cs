using Eval2.Models.Service;
using Eval2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Eval2.Data;
using Microsoft.EntityFrameworkCore;

namespace Eval2.Controllers
{
    public class AdminController : Controller
    {
        private readonly EquipeService equipeService;
        private readonly AdminService adminService;

        private readonly DataContext _datacontext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminController(EquipeService equipeService, DataContext datacontext,
            IHttpContextAccessor httpContextAccessor, AdminService admin)
        {
            this.equipeService = equipeService;
            _datacontext = datacontext;
            _httpContextAccessor = httpContextAccessor;
            this.adminService = admin;
        }



        // GET: AdminController
        public async  Task<ActionResult> Index()
        {
            var currentUser = await adminService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            ViewBag.useradmin = currentUser;
            var coureurs = await _datacontext.coureur
                .Include(c => c.equipe)
                .ToListAsync();
            return View(coureurs);
        }

        public IActionResult Login()
        {
            return View();
        }
        

        public async Task<IActionResult> CheckLogin(Admin admin)
        {
            Console.WriteLine("client avy any am input " + admin.email);

            var adm = await adminService.CheckLoginAsync(admin.email, admin.mdp);

            //Console.WriteLine("client azo avy amle fonction checklogin: " + adm.email);
            try
            {
                if (adm != null)
                {
                    string role = "Admin";

                    Console.WriteLine("role: " + role);
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, adm.idadmin.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        // Indique à ASP.NET Core de conserver l'authentification même après la fermeture du navigateur
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Accueil");
                }
                else
                {
                    throw new Exception("Email ou Mot De Passe Invalide");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("ERROR: ", ex);
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> Accueil()
        {

            var currentUser = await adminService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            ViewBag.useradmin = currentUser;
            var coureurs = await _datacontext.coureur
                .Include(c => c.equipe)
                .ToListAsync();
            return View(coureurs);
        }

       
        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
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

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
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

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
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
