using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Backend.Models.Entities
{
        public class DocumentShare 
        {
            public Guid Id { get; set; }
            public Guid DocumentId { get; set; }
            public DocumentViewModel Document { get; set; } = null!;
            public Guid UserId { get; set; }
            public User User { get; set; } = null!;
        }
}
