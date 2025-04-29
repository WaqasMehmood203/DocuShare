using System.ComponentModel.DataAnnotations;

namespace DMS.Backend.API.Service.Dtos.ExternalStorages
{
    public class UpdateExternalStorageDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public string? GoogleDriveAccessToken { get; set; }
        public string? OneDriveAccessToken { get; set; }

        [Required]
        public DateTime TokenExpiry { get; set; }
    }
}
