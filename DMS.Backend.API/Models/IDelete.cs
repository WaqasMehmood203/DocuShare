﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Backend.Models
{
    public interface IDelete
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
