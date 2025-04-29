namespace DMS.Backend.API.Service.Dtos.Comments
{
    public class CommentParametersDto : ParametersDto
    {
        public Guid? UserId { get; set; }
        public Guid? DocumentId { get; set; }
        public string? Content { get; set; }
    }
}
