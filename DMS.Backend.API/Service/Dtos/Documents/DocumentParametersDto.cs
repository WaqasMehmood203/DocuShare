namespace DMS.Backend.API.Service.Dtos.Documents
{
    public class DocumentParametersDto : ParametersDto
    {
        public string? Title { get; set; }
        public string? Visibility { get; set; }
        public Guid? OwnerId { get; set; }
    }
}
