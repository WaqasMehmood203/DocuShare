namespace DMS.Backend.API.Service.Dtos.Users
{
    public class UserParametersDto : ParametersDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public bool? IsActive { get; set; }
    }
}
