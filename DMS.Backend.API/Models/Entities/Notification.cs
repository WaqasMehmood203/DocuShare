using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Backend.Models.Entities
{
    public class Notification : BaseModel
    {
        public Guid Id { get; set; }
        public Guid ReceiverId { get; set; }
        public User Receiver { get; set; } = null!;
        public string Message { get; set; } = string.Empty;

        public Enums.NotificationType Type { get; set; }
        public bool IsRead { get; set; }
    }
}
