using Eval2.Data;
using Microsoft.EntityFrameworkCore;

namespace Eval2.Models.Service
{
    public class CoureurService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DataContext _context;

        public CoureurService(IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IEnumerable<Coureurs>> getCoureurByEquipe(int id)
        {
            var coureurs = await _context.coureur.Where(c=>c.idequipe==id).ToListAsync();
            return coureurs;
        }

    }
}
