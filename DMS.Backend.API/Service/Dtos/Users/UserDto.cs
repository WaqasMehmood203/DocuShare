using DMS.Backend.API.Models;
using DMS.Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace DMS.Backend.API.Service.Dtos.Users
{
    public class UserDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Range(0, 120)]
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
