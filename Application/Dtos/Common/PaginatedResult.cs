namespace Application.Dtos.Common
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Total { get; set; }
        public int CurrentPage { get; set; }
        public int PerPage { get; set; }
        public int LastPage => (int)Math.Ceiling((double)Total / PerPage);
        public int From => (CurrentPage - 1) * PerPage + 1;
        public int To => Math.Min(CurrentPage * PerPage, Total);
    }
}