using System.ComponentModel.DataAnnotations;

namespace DMS.Backend.API.Service.Dtos.DocumentTags
{
    public class UpdateDocumentTagDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string TagName { get; set; }

        [Required]
        public Guid DocumentId { get; set; }
    }
}
