namespace DMS.Backend.API.Service.Dtos.Friends
{
    public class FriendParametersDto : ParametersDto
    {
        public string? UserId { get; set; }
        public string? FriendId { get; set; }
    }
}
