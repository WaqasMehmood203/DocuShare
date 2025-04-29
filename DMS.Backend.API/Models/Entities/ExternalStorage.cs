using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Backend.Models.Entities
{
    public class ExternalStorage 
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public string? GoogleDriveAccessToken { get; set; }
        public string? OneDriveAccessToken { get; set; }
        public DateTime TokenExpiry { get; set; }
    }
}
