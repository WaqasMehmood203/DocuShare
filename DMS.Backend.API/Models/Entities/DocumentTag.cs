using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Backend.Models.Entities
{
    public class DocumentTag 
    {
        public Guid Id { get; set; }
        public string TagName { get; set; } = string.Empty;
        public Guid DocumentId { get; set; }
        public Document Document { get; set; } = null!;
    }
}
