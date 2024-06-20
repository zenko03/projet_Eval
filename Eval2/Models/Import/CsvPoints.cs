using Eval2.Data;
using Eval2.Models.outils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Eval2.Models.Import
{
    public class CsvPoints
    {
        [Key]
        public int id {  get; set; }
        public int classement { get; set; }
        public int points { get; set; }

        public ImportCsvResult<CsvPoints> GetCsvResult(List<CsvPointsLine> lines)
        {
            Console.WriteLine("Nombre csv line: " + lines.Count);

            List<CsvPoints> listPoints = new List<CsvPoints>();
            List<LineError> lineErrors = new List<LineError>();
            foreach (CsvPointsLine line in lines)
            {
                try
                {
                    CsvPoints points = new CsvPoints
                    {
                        classement = (int)Validation.ValidateInt(line.classement.Trim()),
                        points = (int)Validation.ValidateInt(line.points.Trim())
                    };

                    listPoints.Add(points);
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
            return new ImportCsvResult<CsvPoints>(listPoints, lineErrors);
        }

        public async Task<ImportCsvResult<CsvPoints>> DispatchToTableAsync(DataContext context, IWebHostEnvironment hostEnvironment, string csvFolder, IFormFile file)
        {
            Console.WriteLine("MIANTSO AN DISPATCH TO TABLE AN'I CSVPOINTS");

            ImportCsvResult<CsvPoints> result = new ImportCsvResult<CsvPoints>(new List<CsvPoints>(), new List<LineError>());
            bool uploaded = await Fonctions.UploadCsvFile(hostEnvironment, csvFolder, file);

            if (uploaded)
            {
                List<CsvPointsLine> pointsFromCsv = Fonctions.ReadCsv<CsvPointsLine>(hostEnvironment, csvFolder, file.FileName);
                result = this.GetCsvResult(pointsFromCsv);

                List<CsvPoints> csvPoints = result.ListeObject;

                context.csvPoints.RemoveRange(context.csvPoints);
                context.csvPoints.AddRange(csvPoints);
                await context.SaveChangesAsync();

                var pointEtapeQuery =
                      from pt in csvPoints
                      group pt by new { pt.classement, pt.points } into g
                      select new
                      {
                          Rang = g.Key.classement,
                          Points = g.Key.points
                      };

                foreach (var pointEtape in pointEtapeQuery)
                {
                    if (!context.pointEtapes.Any(pe => pe.rang == pointEtape.Rang))
                    {
                        context.pointEtapes.Add(new PointEtape
                        {
                            rang = pointEtape.Rang,
                            point = pointEtape.Points
                        });
                    }
                }

                await context.SaveChangesAsync();
            }

            return result;
        }
    }
}
