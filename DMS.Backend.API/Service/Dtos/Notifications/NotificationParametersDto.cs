namespace DMS.Backend.API.Service.Dtos.Notifications
{
    public class NotificationParametersDto : ParametersDto
    {
        public Guid? ReceiverId { get; set; }
        public bool? IsRead { get; set; }
        public string? Type { get; set; }
    }
}
