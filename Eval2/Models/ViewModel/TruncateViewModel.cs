namespace Eval2.Models.ViewModel
{
    public class TruncateViewModel
    {
        public string Table { get; set; }

        public TruncateViewModel(string table)
        {
            Table = table;
        }
    }
}
