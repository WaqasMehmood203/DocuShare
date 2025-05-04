using Azure;
using DMS.Backend.API.Models.Entities;
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
            public Guid DocumentId { get; set; }
            public Document Document { get; set; }
            public Guid TagId { get; set; }
            public Tag Tag { get; set; }

    }
}
