namespace DMS.Backend.Models.Entities
{
    public class QueryParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchString { get; set; } = "";
        public string Visibility { get; set; } = "";
        public string Tags { get; set; } = "";
        public string SortBy { get; set; } = "CreatedDate";
        public string SortOrder { get; set; } = "desc";

        private const int MaxPageSize = 50;
        public int ValidPageSize => PageSize <= 0 || PageSize > MaxPageSize ? 10 : PageSize;
    }
}
