using System.ComponentModel.DataAnnotations;

namespace DMS.Backend.API.Service.Dtos.DocumentShares
{
    public class DocumentShareParametersDto : ParametersDto
    {
        [Required]
        public string TagName { get; set; }

        [Required]
        public Guid DocumentId { get; set; }
    }
}
