namespace DMS.Backend.API.Service.Dtos.Likes
{
    public class LikeParametersDto : ParametersDto
    {
        public Guid? UserId { get; set; }
        public Guid? DocumentId { get; set; }
    }
}
