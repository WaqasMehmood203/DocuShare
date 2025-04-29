namespace DMS.Backend.API.Service.Dtos
{
    public class ParametersDto
    {
        public List<string>? FilterOn { get; set; }
        public string SortBy { get; set; } = "Id";
        public bool IsAscending { get; set; } = true;
        public int PageNumber { get; set; } = 1; // Default to page 1
        public int PageSize { get; set; } = 1000; // Default to 1000 records per page
    }
}
