using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static DMS.Backend.Models.Enums;

namespace DMS.Backend.Models.Entities
{
    public class DocumentViewModel : BaseModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
        public DocumentVisibility Visibility { get; set; }
        public string Tags { get; set; }

        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<DocumentShare> DocumentShares { get; set; } = new List<DocumentShare>();
    }
}
