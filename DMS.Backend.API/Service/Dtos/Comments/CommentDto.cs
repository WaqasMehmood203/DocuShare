using System.ComponentModel.DataAnnotations;

namespace DMS.Backend.API.Service.Dtos.Comments
{
    public class CommentDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid DocumentId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
    }
}
