using Eval2.Data;
using Eval2.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Eval2.Models.Service
{
    public class CoureurEtapeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DataContext _context;

        public CoureurEtapeService(IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        // Vérifie si le nombre de coureurs requis est atteint dans l'étape
        public async Task<bool> IsEtapeFullAsync(int etapeId)
        {
            var etape = await _context.etape.FindAsync(etapeId);
            if (etape == null)
            {
                throw new KeyNotFoundException("Étape non trouvée.");
            }

            var currentCount = await _context.coureurEtapes.CountAsync(ce => ce.idetape == etapeId);
            return currentCount >= etape.nbrcoureur;
        }

        // Vérifie si le coureur est déjà affecté à l'étape
        public async Task<bool> IsCoureurInEtapeAsync(int etapeId, int coureurId)
        {
            return await _context.coureurEtapes.AnyAsync(ce => ce.idetape == etapeId && ce.idcoureur == coureurId);
        }

        // Affecte le coureur à l'étape
        public async Task AddCoureurToEtapeAsync(int etapeId, int coureurId)
        {
            if (await IsEtapeFullAsync(etapeId))
            {
                throw new InvalidOperationException("Le nombre maximum de coureurs pour cette étape est atteint.");
            }

            if (await IsCoureurInEtapeAsync(etapeId, coureurId))
            {
                throw new InvalidOperationException("Le coureur est déjà affecté à cette étape.");
            }

            var coureurEtape = new CoureurEtape
            {
                idetape = etapeId,
                idcoureur = coureurId
            };

            _context.coureurEtapes.Add(coureurEtape);
            await _context.SaveChangesAsync();
        }

        public CoureurEtapeService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<CoureurEtape>> GetCoureursByEtapeIdAsync(int etapeId)
        {
            return await _context.coureurEtapes
                .Include(ce => ce.coureur)
                    .ThenInclude(c => c.equipe)
                .Include(ce => ce.etape)
                .Where(ce => ce.idetape == etapeId)
                .OrderBy(ce => ce.coureur.equipe.nom)
                .ToListAsync();
        }


    }
}
