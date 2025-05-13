using DMS.Backend.API.Models;
using DMS.Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace DMS.Backend.API.Service.Dtos.Documents
{
    public class UpdateDocumentDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [EnumDataType(typeof(Enums.DocumentVisibility))]
        public Enums.DocumentVisibility Visibility { get; set; }
    }
}
