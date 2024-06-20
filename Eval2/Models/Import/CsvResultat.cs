using Eval2.Data;
using Eval2.Models.outils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models.Import
{
    [Serializable]
    public class CsvResultat
    {
        [Key]
        public int id {  get; set; }
        public int etaperang { get; set; }
        public int numerodossard { get; set; }
        public string nom { get; set; }
        public string genre { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime datenaissance { get; set; }
        public string equipe { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime arrivee { get; set; }

        public ImportCsvResult<CsvResultat> GetCsvResult(List<CsvResultatLine> lines)
        {
            Console.WriteLine("Nombre csv line: " + lines.Count);

            List<CsvResultat> listEtapes = new List<CsvResultat>();
            List<LineError> lineErrors = new List<LineError>();
            foreach (CsvResultatLine line in lines)
            {
                try
                {
                    CsvResultat etape = new CsvResultat
                    {
                        etaperang = (int)Validation.ValidateInt(line.etaperang),
                        numerodossard = (int)Validation.ValidateInt(line.numerodossard),
                        nom = Validation.ValidateString(line.nom),
                        genre = Validation.ValidateString(line.genre),
                        datenaissance = Validation.FormatDate(line.datenaissance.Trim()),
                        equipe = Validation.ValidateString(line.equipe),
                        arrivee = Validation.FormatDate(line.arrivee)
                    };

                    listEtapes.Add(etape);
                }
                catch (Exception e)
                {
                    Console.WriteLine("----------------------------- ERROR -------------------------------");
                    Console.Error.WriteLine($"LINE PARSE ERROR => Ligne {lines.IndexOf(line) + 1}, {e.Message}");
                    Console.Error.WriteLine($"LINE PARSE ERROR => Ligne {lines.IndexOf(line) + 1}, {e.StackTrace}");
                    Console.WriteLine("----------------------------- ERROR -------------------------------");
                    lineErrors.Add(new LineError(lines.IndexOf(line) + 1, e.Message));
                    continue;
                }
            }
            return new ImportCsvResult<CsvResultat>(listEtapes, lineErrors);
        }

        public async Task<ImportCsvResult<CsvResultat>> DispatchToTableAsync(DataContext context, IWebHostEnvironment hostEnvironment, string csvFolder, IFormFile file)
        {
            Console.WriteLine("MIANTSO AN DISPATCH TO TABLE AN'I CSVPOINTS");

            ImportCsvResult<CsvResultat> result = new ImportCsvResult<CsvResultat>(new List<CsvResultat>(), new List<LineError>());
            bool uploaded = await Fonctions.UploadCsvFile(hostEnvironment, csvFolder, file);

            if (uploaded)
            {
                List<CsvResultatLine> resultatsFromCsv = Fonctions.ReadCsv<CsvResultatLine>(hostEnvironment, csvFolder, file.FileName);
                result = this.GetCsvResult(resultatsFromCsv);

                List<CsvResultat> csvResultats = result.ListeObject;

                // Insérer les données CSV dans la table 'CsvResultat'
                context.csvResultats.RemoveRange(context.csvResultats); // Supprimer les anciennes données
                context.csvResultats.AddRange(csvResultats); // Ajouter les nouvelles données
                await context.SaveChangesAsync();

                // Insérer les données dans les tables associées
                // Insérer ou mettre à jour les équipes
                var equipeQuery =
                    from cr in csvResultats
                    group cr by cr.equipe into g
                    select new Equipe
                    {
                        nom = g.Key,
                        email = g.Key,
                        mdp = g.Key,
                        etat = 0

                    };

                foreach (var equipe in equipeQuery)
                {
                    if (!context.equipe.Any(e => e.nom == equipe.nom))
                    {
                        context.equipe.Add(equipe);
                    }
                }

                await context.SaveChangesAsync();

                // Insérer ou mettre à jour les coureurs
                var coureurQuery =
                    from cr in csvResultats
                    group cr by new { cr.nom, cr.genre, cr.datenaissance, cr.numerodossard, cr.equipe } into g
                    select new
                    {
                        Nom = g.Key.nom,
                        Genre = g.Key.genre,
                        DateNaissance = g.Key.datenaissance,
                        NumeroDossard = g.Key.numerodossard,
                        EquipeId = context.equipe.FirstOrDefault(e => e.nom == g.Key.equipe)?.idequipe
                    };

                foreach (var coureur in coureurQuery)
                {
                    if (!context.coureur.Any(c => c.nom == coureur.Nom && c.numdossard == coureur.NumeroDossard))
                    {
                        context.coureur.Add(new Coureurs
                        {
                            nom = coureur.Nom,
                            genre = coureur.Genre,
                            ddn = coureur.DateNaissance,
                            numdossard = coureur.NumeroDossard,
                            idequipe = (int)coureur.EquipeId
                        });
                    }
                }
                await context.SaveChangesAsync();


                // Insérer ou mettre à jour les relations CoureurEtape
                var coureurEtapeQuery =
                    from cr in csvResultats
                    join et in context.etape on cr.etaperang equals et.rangetape
                    join c in context.coureur on cr.numerodossard equals c.numdossard
                    select new
                    {
                        CoureurId = c.idcoureur,
                        EtapeId = et.idetape
                    };

                foreach (var ce in coureurEtapeQuery)
                {
                    if (!context.coureurEtapes.Any(ce => ce.idcoureur == ce.idcoureur && ce.idetape == ce.idetape))
                    {
                        context.coureurEtapes.Add(new CoureurEtape
                        {
                            idcoureur = ce.CoureurId,
                            idetape = ce.EtapeId
                        });
                    }
                }

                await context.SaveChangesAsync();

                // Insérer ou mettre à jour les résultats
                var resultatQuery =
                    from cr in csvResultats
                    join et in context.etape on cr.etaperang equals et.rangetape
                    join c in context.coureur on cr.numerodossard equals c.numdossard
                    select new ResultatEtape
                    {
                        idetape = et.idetape,
                        idcoureur = c.idcoureur,
                        tempsarriver = cr.arrivee
                    };

                foreach (var resultat in resultatQuery)
                {
                    if (!context.resultat.Any(r => r.idetape == resultat.idetape && r.idcoureur == resultat.idcoureur))
                    {
                        context.resultat.Add(resultat);
                    }
                }

                await context.SaveChangesAsync();
            }

            return result;
        }
    }
}
