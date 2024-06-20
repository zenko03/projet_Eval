using CsvHelper;
using CsvHelper.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
namespace Eval2.Models.outils
{
    public class Fonctions
    {
        public static List<T> ReadCsv<T>(IWebHostEnvironment hostEnvironment, string csvFolder, string fileName)
        {
            List<T> csvLines = new List<T>();

            var path = Path.Combine(hostEnvironment.WebRootPath, csvFolder, fileName);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
                BadDataFound = context => { Console.WriteLine($"Bad data at row '{context.RawRecord}'"); }, // Gérer les données incorrectes
                MissingFieldFound = null
            };
            Console.WriteLine($"PATH DU CSV {fileName} => {path}");
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, csvConfig))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var csvLine = csv.GetRecord<T>();
                    TrimStringProperties(csvLine);
                    Console.WriteLine($"Add CSV LINE: {csvLine}");
                    csvLines.Add(csvLine);
                }
            }
            Console.WriteLine("**** NOMBRE FINAL ==>" + csvLines.Count);

            return csvLines;
        }

        public static async Task<bool> UploadCsvFile(IWebHostEnvironment hostEnvironment, string yourFolder, IFormFile file)
        {
            try
            {
                string fileDir = Path.Combine(hostEnvironment.WebRootPath, yourFolder);
                Console.WriteLine($"File dir >>> {fileDir}");
                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }
                string fileName = Path.Combine(fileDir, file.FileName);
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR UPLOAD: " + e.StackTrace);
                throw e;
            }
            return true;
        }

        public static List<T> ReadXls<T>(string filePath) where T : new()
        {
            List<T> list = new List<T>();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; // Nécessaire pour EPPlus 5.x et versions ultérieures
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++)
                {
                    var obj = new T();
                    for (int col = 1; col <= colCount; col++)
                    {
                        var prop = typeof(T).GetProperties()[col - 1];
                        var cellValue = worksheet.Cells[row, col].Text;
                        prop.SetValue(obj, Convert.ChangeType(cellValue, prop.PropertyType));
                    }
                    list.Add(obj);
                }
            }
            return list;
        }

        public static async Task<bool> UploadXlsFile(IWebHostEnvironment hostEnvironment, String folder, IFormFile file)
        {
            try
            {
                string fileDir = Path.Combine(hostEnvironment.WebRootPath, folder);
                Console.WriteLine($"File dir >>> {fileDir}");
                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }
                string fileName = Path.Combine(fileDir, file.FileName);
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR UPLOAD: " + e.StackTrace);
                throw e;
            }
            return true;
        }

        private static void TrimStringProperties<T>(T obj)
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string)property.GetValue(obj);
                    if (value != null)
                    {
                        value = value.Trim();
                        property.SetValue(obj, value);
                    }
                }
            }
        }
    }
}
