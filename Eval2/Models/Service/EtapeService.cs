using Eval2.Data;
using Microsoft.EntityFrameworkCore;

namespace Eval2.Models.Service
{
    public class EtapeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DataContext _context;

        public EtapeService(IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IEnumerable<Etape>> GetPageAsync(int pageNumber, int pageSize)
        {
            return await _context.etape
                .Include(e => e.course)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public int GetTotalPages(int pageSize)
        {
            int totalItems = _context.etape.Count();
            return (int)Math.Ceiling((double)totalItems / pageSize);
        }

        public async Task<Etape> GetEtapeByIdAsync(int id)
        {
            var etape = await _context.etape
                .Include(e => e.course) 
                .FirstOrDefaultAsync(e => e.idetape == id);
            return etape;
        }
    }
}
