using DMS.Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace DMS.Backend.API.Service.Dtos.FriendRequests
{
    public class FriendRequestDto
    {
        [Required]
        public string SenderId { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        [EnumDataType(typeof(Enums.FriendRequestStatus))]
        public Enums.FriendRequestStatus Status { get; set; }
    }
}
