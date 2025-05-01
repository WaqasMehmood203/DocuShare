using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Backend.Models.Entities
{
    public class User 
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [RegularExpression(
        @"^[^@\s]+@gmail\.com$",
        ErrorMessage = "Email must be a Gmail address (e.g. you@gmail.com)."
    )]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8,
        ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(
        @"^(?=.*[!@#$%^&*])(?=.{8,}).*$",
        ErrorMessage = "Password must contain at least one special character (e.g. @#$%^&*)."
    )]
        public string Password { get; set; } = string.Empty;
        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public int? Age { get; set; }
        public Enums.Gender? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProfileImage { get; set; }
        public bool? IsActive { get; set; }

        public ICollection<Friend> Friends { get; set; }
        public ICollection<FriendRequest> SentFriendRequests { get; set; }
        public ICollection<FriendRequest> ReceivedFriendRequests { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<DocumentShare> DocumentShares { get; set; }
    }
}
