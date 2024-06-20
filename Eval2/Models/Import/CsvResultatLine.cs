using CsvHelper.Configuration.Attributes;
using Eval2.Data;
using Eval2.Models.outils;

namespace Eval2.Models.Import
{
    public class CsvResultatLine
    {
        [Index(0)]
        public string etaperang { get; set; }
        [Index(1)]
        public string numerodossard { get; set; }
        [Index(2)]
        public string nom { get; set; }
        [Index(3)]
        public string genre { get; set; }
        [Index(4)]
        public string datenaissance { get; set; }
        [Index(5)]
        public string equipe { get; set; }
        [Index(6)]
        public string arrivee { get; set; }

        
    }
}
