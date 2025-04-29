using System.ComponentModel.DataAnnotations;

namespace DMS.Backend.API.Service.Dtos.Friends
{
    public class UpdateFriendDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string FriendId { get; set; }
    }
}
