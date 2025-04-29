namespace DMS.Backend.API.Service.Dtos.DocumentTags
{
    public class DocumentTagParametersDto : ParametersDto
    {
        public string? TagName { get; set; }
        public Guid? DocumentId { get; set; }
    }
}
