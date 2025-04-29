using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Backend.Models
{
    public interface IUpdate
    {
        public bool IsUpdated { get; set; } 
        public DateTime? UpdatedDate { get; set; }
    }
}
