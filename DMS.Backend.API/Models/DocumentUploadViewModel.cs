using System.ComponentModel.DataAnnotations;

namespace DMS.Backend.API.Models
{
    public class DocumentUploadViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public IFormFile File { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public Backend.Models.Enums.DocumentVisibility Visibility { get; set; }
    }
}
