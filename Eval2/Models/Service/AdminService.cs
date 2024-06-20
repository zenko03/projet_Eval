using Eval2.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Eval2.Models.Service
{
    public class AdminService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DataContext _context;

        public AdminService(IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<Admin> CheckLoginAsync(string email, string password)
        {
            try
            {
                var admin = await _context.admin
                    .FirstOrDefaultAsync(e => e.email == email && e.mdp == password);

                return admin;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<Admin> GetCurrentUserAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            return await _context.admin.FindAsync(int.Parse(userId));
        }

        public async Task<IEnumerable<Etape>> GetAllEtapes()
        {
            return await _context.etape.Include(c => c.course).ToListAsync();
        }
    }
}
