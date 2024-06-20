using Eval2.Data;
using Microsoft.EntityFrameworkCore;

namespace Eval2.Models.Service
{
    public class ResultatEtapeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DataContext _context;

        public ResultatEtapeService(IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<ResultatEtape> GetResultatByCoureurEtapeAsync(int coureurId, int etapeId)
        {
            return await _context.resultat
                .FirstOrDefaultAsync(re => re.idcoureur == coureurId && re.idetape == etapeId);
        }

        public async Task AddOrUpdateResultatAsync(ResultatEtape resultatEtape)
        {
            var existingResultat = await GetResultatByCoureurEtapeAsync(resultatEtape.idcoureur, resultatEtape.idetape);

            if (existingResultat == null)
            {
                _context.resultat.Add(resultatEtape);
            }
            else
            {
                existingResultat.tempsarriver = resultatEtape.tempsarriver;
                _context.resultat.Update(existingResultat);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteResultatAsync(int coureurId, int etapeId)
        {
            var existingResultat = await GetResultatByCoureurEtapeAsync(coureurId, etapeId);

            if (existingResultat != null)
            {
                _context.resultat.Remove(existingResultat);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<DateTime?> GetTempsArriverAsync(int coureurId, int etapeId)
        {
            var resultat = await _context.resultat
       .FirstOrDefaultAsync(re => re.idcoureur == coureurId && re.idetape == etapeId);

            if (resultat != null)
            {
                return resultat.tempsarriver;
            }
            else
            {
                return DateTime.Today; // Par défaut, retourne aujourd'hui à 00:00:00
            }
        }

        public async Task<TimeSpan?> GetChronoTimeAsync(int etapeId, int coureurId)
        {
            var resultatEtape = await _context.resultat
                .FirstOrDefaultAsync(re => re.idetape == etapeId && re.idcoureur == coureurId);

            if (resultatEtape == null)
            {
                return null;
            }

            var etape = await _context.etape.FindAsync(etapeId);

            if (etape == null || etape.tempsdepart == null)
            {
                return null;
            }

            var tempsArriver = resultatEtape.tempsarriver;
            var tempsDepart = etape.tempsdepart;

            return tempsArriver - tempsDepart;
        }
    }
}
