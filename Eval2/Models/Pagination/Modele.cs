namespace Eval2.Models.Pagination
{
    public class Modele<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
