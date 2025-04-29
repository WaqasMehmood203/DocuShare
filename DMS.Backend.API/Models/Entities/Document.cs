using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DMS.Backend.Models.Entities
{
    public class Document : BaseModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;
        public string Content { get; set; } = string.Empty;
        public Enums.DocumentVisibility Visibility { get; set; }

        public ICollection<DocumentTag> Tags { get; set; } = new List<DocumentTag>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<DocumentShare> DocumentShares { get; set; } = new List<DocumentShare>();
    }
}
