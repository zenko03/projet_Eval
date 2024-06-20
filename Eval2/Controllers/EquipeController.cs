using Eval2.Data;
using Eval2.Models;
using Eval2.Models.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using Microsoft.Extensions.Hosting;

namespace Eval2.Controllers
{
    public class EquipeController : Controller
    {
        private readonly EquipeService equipeService;

        private readonly DataContext _datacontext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EquipeController(EquipeService equipeService, DataContext datacontext, IHttpContextAccessor httpContextAccessor)
        {
            this.equipeService = equipeService;
            _datacontext = datacontext;
            _httpContextAccessor = httpContextAccessor;
        }


        // GET: Equipe
        public ActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            
            return View();
        }

        public async Task<IActionResult> CheckLogin(Equipe equipe)
        {
            Console.WriteLine("client avy any am input " + equipe.email);

            var equip = await equipeService.CheckLoginAsync(equipe.email, equipe.mdp);

            Console.WriteLine("client azo avy amle fonction checklogin: " + equip.nom);
          try {
            if (equip != null)
            {
                string role = "Equipe";

                Console.WriteLine("role: " + role);
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, equip.idequipe.ToString()),
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
          }catch(Exception ex)
            {
                Console.Error.WriteLine("ERROR: ", ex);
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> Accueil()
        {

            var currentUser = await equipeService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Equipe");
            }

            ViewBag.user = currentUser;

            return View(); 
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Register()
        {
           
            return View();
        }

        public async Task<IActionResult> EquipeSignup(Equipe equipe)
        {
            //if (!Validation.IsValidPassword(viewModel.Password) || !viewModel.Password.Equals(viewModel.ConfirmPassword))
            //{
            //    throw new Exception("Mot de passe invalide. Veuillez reconfirmer votre mot de passe");
            //}
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var team = new Equipe
            {
                nom = equipe.nom,
                email = equipe.email,
                mdp = equipe.mdp,
                etat = 0
            };
            await _datacontext.equipe.AddAsync(team);
            int written = await _datacontext.SaveChangesAsync();
            if (written <= 0)
            {
                ViewBag.ErrorMessage = "User non inseré !";
            }
            else
            {
                ViewBag.SuccessMessage = "yesss";
            }
            return RedirectToAction("Login");
        }
        // GET: Equipe/Details/5S
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Equipe/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Equipe/Create
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

        // GET: Equipe/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Equipe/Edit/5
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

        // GET: Equipe/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Equipe/Delete/5
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
