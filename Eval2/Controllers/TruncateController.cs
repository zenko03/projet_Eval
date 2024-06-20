using Eval2.Controllers;
using Eval2.Data;
using Eval2.Models.Service;
using Eval2.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace Evaluation.Controllers
{
    [AllowAnonymous]
    public class TruncateController : Controller
    {
        private readonly EquipeService equipeService;
        private readonly AdminService adminService;
        private readonly ClassementService classementServices;


        private readonly DataContext _datacontext;
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] _excludedTables = new string[] { "admin", "categorie" }; // Replace with excluded table names

        public TruncateController(DataContext dataContext,AdminService adminServices, ClassementService classementService)
        {
            _datacontext = dataContext;
            adminService = adminServices;
            classementServices = classementService;
        }


        public IActionResult Index()
        {
            var tables = _datacontext.Model.GetEntityTypes().Select(t => t.GetTableName()).ToList();
            return View(tables);
        }

        [HttpPost]
        public async Task<IActionResult> ClearTable(string tableName)
        {
            var table = _datacontext.Model.GetEntityTypes().FirstOrDefault(t => t.GetTableName() == tableName);
            if (table != null)
            {
                var entityType = table.ClrType;
                var dbSet = _datacontext.GetType().GetMethod("Set").MakeGenericMethod(entityType).Invoke(_datacontext, null);
                var removeRangeMethod = typeof(DbContext).GetMethod("RemoveRange", new[] { typeof(System.Collections.IEnumerable) });
                removeRangeMethod.Invoke(_datacontext, new[] { dbSet });
                await _datacontext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ClearAllTables()
        {
            var tables = _datacontext.Model.GetEntityTypes().Select(t => t.ClrType).ToList();
            foreach (var table in tables)
            {
                var dbSet = _datacontext.GetType().GetMethod("Set").MakeGenericMethod(table).Invoke(_datacontext, null);
                var removeRangeMethod = typeof(DbContext).GetMethod("RemoveRange", new[] { typeof(System.Collections.IEnumerable) });
                removeRangeMethod.Invoke(_datacontext, new[] { dbSet });
            }
            await _datacontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
