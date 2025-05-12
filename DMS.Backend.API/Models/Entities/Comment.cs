using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Backend.Models.Entities
{
    public class Comment : BaseModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public Guid DocumentId { get; set; }

        public Document Document { get; set; } = null!;

        [Required]
        public string Content { get; set; } = string.Empty;

        public Guid? ParentCommentId { get; set; } // Nullable for top-level comments

        public Comment? ParentComment { get; set; } // Navigation to parent comment

        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
