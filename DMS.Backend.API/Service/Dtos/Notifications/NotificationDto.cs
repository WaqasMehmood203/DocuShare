using DMS.Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace DMS.Backend.API.Service.Dtos.Notifications
{
    public class NotificationDto
    {
        [Required]
        public Guid ReceiverId { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        [EnumDataType(typeof(Enums.NotificationType))]
        public Enums.NotificationType Type { get; set; }
        public bool IsRead { get; set; }
    }
}
