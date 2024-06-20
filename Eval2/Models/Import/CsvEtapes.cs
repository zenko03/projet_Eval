using Eval2.Data;
using Eval2.Models.outils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Eval2.Models.Import
{
    [Serializable]
    public class CsvEtapes
    {
        [Key]
        [Column("id")]
        public int id {  get; set; }
        [Column("etape")]
        public string? etape { get; set; }
        [Column("longueur")]
        public double? longueur { get; set; }
        [Column("nbcoureur")]
        public int? nbcoureur { get; set; }
        [Column("rang")]
        public int? rang { get; set; }
        [Column("datedepart")]
        public DateTime datedepart { get; set; }

        public CsvEtapes() { }


        public ImportCsvResult<CsvEtapes> GetCsvResult(List<CsvEtapeLine> lines)
        {
            Console.WriteLine("Nombre csv line: " + lines.Count);

            List<CsvEtapes> listEtapes = new List<CsvEtapes>();
            List<LineError> lineErrors = new List<LineError>();
            foreach (CsvEtapeLine line in lines)
            {
                Console.WriteLine("datedeb " + line.datedepart + " heuredep " + line.heuredepart);

                try
                {
                    // Vérification si datedepart et heuredepart ne sont pas null ni des chaînes vides
                    if (!string.IsNullOrEmpty(line.datedepart) && !string.IsNullOrEmpty(line.heuredepart))
                    {
                        string dateHeureStr = $"{line.datedepart.Trim()} {line.heuredepart.Trim()}";
                        if (DateTime.TryParse(dateHeureStr, out DateTime parsedDate))
                        {
                            CsvEtapes etape = new CsvEtapes
                            {
                                etape = Validation.ValidateString(line.etape),
                                longueur = Validation.ValidateDouble(line.longueur.Trim()),
                                nbcoureur = Validation.ValidateInt(line.nbcoureur.Trim()),
                                rang = Validation.ValidateInt(line.rang.Trim()),
                                datedepart = parsedDate // Utilisation de la date parsée
                            };
                            Console.WriteLine("dateparse " + etape.datedepart);
                            Console.WriteLine("datedeb " + line.datedepart + " heuredep " + line.heuredepart);

                            listEtapes.Add(etape);
                        }
                        else
                        {
                            // Gestion de l'erreur de parsing de date
                            Console.WriteLine("----------------------------- ERROR -------------------------------");
                            Console.Error.WriteLine($"DATE PARSING ERROR => Ligne {lines.IndexOf(line) + 1}, Date invalide '{dateHeureStr}'");
                            Console.WriteLine("----------------------------- ERROR -------------------------------");
                            lineErrors.Add(new LineError(lines.IndexOf(line) + 1, $"Date invalide '{dateHeureStr}'"));
                            continue;
                        }
                    }
                    else
                    {
                        // Gestion de l'erreur de date ou heure départ manquant
                        Console.WriteLine("----------------------------- ERROR -------------------------------");
                        Console.Error.WriteLine($"MISSING DATE OR TIME DEPART => Ligne {lines.IndexOf(line) + 1}, Date ou heure départ manquante.");
                        Console.WriteLine("----------------------------- ERROR -------------------------------");
                        lineErrors.Add(new LineError(lines.IndexOf(line) + 1, "Date ou heure départ manquante."));
                        continue;
                    }
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
            return new ImportCsvResult<CsvEtapes>(listEtapes, lineErrors);
        }


        public async Task<ImportCsvResult<CsvEtapes>> DispatchToTableAsync(DataContext context, IWebHostEnvironment hostEnvironment, string csvFolder, IFormFile file)
        {
            Console.WriteLine("MIANTSO AN DISPATCH TO TABLE AN'I CSVPOINTS");

            ImportCsvResult<CsvEtapes> result = new ImportCsvResult<CsvEtapes>(new List<CsvEtapes>(), new List<LineError>());
            bool uploaded = await Fonctions.UploadCsvFile(hostEnvironment, csvFolder, file);

            if (uploaded)
            {
                List<CsvEtapeLine> etapesFromCsv = Fonctions.ReadCsv<CsvEtapeLine>(hostEnvironment, csvFolder, file.FileName);
                result = this.GetCsvResult(etapesFromCsv);

                List<CsvEtapes> csvEtapes = result.ListeObject;

                context.csvEtapes.RemoveRange(context.csvEtapes);
                context.csvEtapes.AddRange(csvEtapes);

                // Utilisation de LINQ pour insérer les données
                var uniqueEtapes = csvEtapes.GroupBy(e => new { e.etape, e.rang })
                                           .Select(g => new
                                           {
                                               Etape = g.Key.etape,
                                               Longueur = g.Average(x => x.longueur),
                                               NbCoureur = g.Average(x => x.nbcoureur),
                                               DateDepart = g.Min(x => x.datedepart),
                                               Rang = g.Key.rang
                                           });

                foreach (var etape in uniqueEtapes)
                {
                    context.etape.Add(new Etape
                    {
                        nom = etape.Etape,
                        distance = etape.Longueur,
                        nbrcoureur = (int?)etape.NbCoureur,
                        rangetape = etape.Rang,
                        tempsdepart = etape.DateDepart
                    });
                }

                await context.SaveChangesAsync();
            }

            return result;
        }



    }
}
