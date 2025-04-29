using System.ComponentModel.DataAnnotations;

namespace DMS.Backend.API.Service.Dtos.Friends
{
    public class FriendDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string FriendId { get; set; }
    }
}
