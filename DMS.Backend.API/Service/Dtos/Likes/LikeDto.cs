using System.ComponentModel.DataAnnotations;

namespace DMS.Backend.API.Service.Dtos.Likes
{
    public class LikeDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid DocumentId { get; set; }
    }
}
