using Eval2.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Eval2.Models.Service
{
    public class EquipeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DataContext _context;

        public EquipeService(DataContext context, IHttpContextAccessor httpContextAccessor) 
        { 
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        
        }

        public async Task<Equipe> CheckLoginAsync(string email, string password)
        {
            try
            {
            var equipe = await _context.equipe
                .FirstOrDefaultAsync(e => e.email == email && e.mdp == password);

                return equipe;
            }catch (Exception ex)
            {

                return null; 
            }
        }

        public async Task<Equipe> GetCurrentUserAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            return await _context.equipe.FindAsync(int.Parse(userId));
        }

        public async  Task<IEnumerable<Etape>> GetAllEtapes()
        {
            return await _context.etape.Include(c => c.course).ToListAsync();
        }

    }
}
