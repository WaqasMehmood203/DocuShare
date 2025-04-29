using System.ComponentModel.DataAnnotations;

namespace DMS.Backend.API.Service.Dtos.DocumentShares
{
    public class DocumentShareDto
    {
        [Required]
        public Guid DocumentId { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
