namespace DMS.Backend.API.Service.Dtos.FriendRequests
{
    public class FriendRequestParametersDto : ParametersDto
    {
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string? Status { get; set; }
    }
}
