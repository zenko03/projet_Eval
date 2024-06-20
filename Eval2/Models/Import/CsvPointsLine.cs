
using CsvHelper.Configuration.Attributes;

namespace Eval2.Models.Import
{
    public class CsvPointsLine
    {
        [Index(0)]
        public string classement {  get; set; }
        [Index(1)]
        public string points { get; set; }
    }
}
