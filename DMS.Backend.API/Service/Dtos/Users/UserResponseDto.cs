using DMS.Backend.API.Models;
using DMS.Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace DMS.Backend.API.Service.Dtos.Users
{
    public class UserResponseDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Range(0, 120)]
        public string Email { get; set; }

        public int Age { get; set; }

        [Required]
        [EnumDataType(typeof(Enums.Gender))]
        public Enums.Gender Gender { get; set; }

        [Required]
        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string? ProfileImage { get; set; }
        public bool IsActive { get; set; }
    }
}
