//using Eval2.Data;
//using Eval2.Models.Service;
//using Eval2.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//public class PenaliteController : Controller
//{
//    private readonly DataContext _context;
//    private readonly EquipeService _equipeService;
//    private readonly AdminService _adminService;
//    private readonly ILogger<PenaliteController> _logger;

//    public PenaliteController(DataContext context, EquipeService equipeService, AdminService adminService, ILogger<PenaliteController> logger)
//    {
//        _context = context;
//        _equipeService = equipeService;
//        _adminService = adminService;
//        _logger = logger;
//    }

//    public async Task<IActionResult> Index()
//    {
//        var currentUser = await _adminService.GetCurrentUserAsync();
//        if (currentUser == null)
//        {
//            return RedirectToAction("Login", "Admin");
//        }

//        ViewBag.useradmin = currentUser;
//        var penalities = await _context.penalities
//            .Include(p => p.equipe)
//            .Include(p => p.etape)
//            .ToListAsync();

//        return View(penalities);
//    }

//    public async Task<IActionResult> AddPena()
//    {
//        var currentUser = await _adminService.GetCurrentUserAsync();
//        if (currentUser == null)
//        {
//            return RedirectToAction("Login", "Admin");
//        }

//        ViewBag.useradmin = currentUser;
//        ViewBag.Equipes = new SelectList(await _context.equipe.ToListAsync(), "idequipe", "nom");
//        ViewBag.Etapes = new SelectList(await _context.etape.ToListAsync(), "idetape", "nom");
//        return View();
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Add(Penalities penalities, string tempspenalite)
//    {
//        var currentUser = await _adminService.GetCurrentUserAsync();
//        if (currentUser == null)
//        {
//            return RedirectToAction("Login", "Admin");
//        }

//        ViewBag.useradmin = currentUser;
//        if (ModelState.IsValid)
//        {
//            if (TimeSpan.TryParse(tempspenalite, out TimeSpan parsedTime))
//            {
//                penalities.tempspenalite = parsedTime.ToString();
//                _context.Add(penalities);
//                try
//                {
//                    await _context.SaveChangesAsync();
//                    TempData["Message"] = "Pénalité ajoutée avec succès.";
//                    TempData["MessageType"] = "success";
//                    return RedirectToAction(nameof(Index));
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "Erreur lors de l'ajout de la pénalité.");
//                    TempData["Message"] = "Erreur lors de l'ajout de la pénalité: " + ex.Message;
//                    TempData["MessageType"] = "error";
//                }
//            }
//            else
//            {
//                ModelState.AddModelError("tempspenalite", "Format de temps invalide.");
//                TempData["Message"] = "Format de temps invalide.";
//                TempData["MessageType"] = "error";
//            }
//        }
//        else
//        {
//            TempData["Message"] = "Erreur lors de l'ajout de la pénalité.";
//            TempData["MessageType"] = "error";
//        }

//        ViewBag.Equipes = new SelectList(await _context.equipe.ToListAsync(), "idequipe", "nom", penalities.idequipe);
//        ViewBag.Etapes = new SelectList(await _context.etape.ToListAsync(), "idetape", "nom", penalities.idetape);
//        return View("AddPena", penalities);
//    }

//    [HttpPost, ActionName("Delete")]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> DeleteConfirmed(int id)
//    {
//        var currentUser = await _adminService.GetCurrentUserAsync();
//        if (currentUser == null)
//        {
//            return RedirectToAction("Login", "Admin");
//        }

//        ViewBag.useradmin = currentUser;
//        var penalities = await _context.penalities.FindAsync(id);
//        if (penalities != null)
//        {
//            _context.penalities.Remove(penalities);
//            await _context.SaveChangesAsync();
//            TempData["Message"] = "Pénalité supprimée avec succès.";
//            TempData["MessageType"] = "success";
//        }
//        else
//        {
//            TempData["Message"] = "Erreur lors de la suppression de la pénalité.";
//            TempData["MessageType"] = "error";
//        }
//        return RedirectToAction(nameof(Index));
//    }
//}
