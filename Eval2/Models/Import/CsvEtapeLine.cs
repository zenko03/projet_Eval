using CsvHelper.Configuration.Attributes;
using Eval2.Models;

namespace Eval2.Models.Import
{
    public class CsvEtapeLine
    {
        [Index(0)]
        public string etape {  get; set; }
        [Index(1)]
        public string longueur { get; set; }
        [Index(2)]
        public string nbcoureur {  get; set; }
        [Index(3)]
        public string rang {  get; set; }
        [Index(4)]
        public string datedepart {  get; set; }
        [Index(5)]
        public string heuredepart {  get; set; }
    }
}


