namespace ApplicationCore.Helper.Models
{
    public class PagingResult
    {
        public virtual Paging Paging { get; set; }
    }

    public class Paging
    {
        public int Page { get; set; }
        public int TotalPage { get; set; }
        public int TotalRow { get; set; }
    }
}
