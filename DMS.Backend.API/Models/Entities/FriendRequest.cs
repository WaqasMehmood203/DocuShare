using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Backend.Models.Entities
{
    public class FriendRequest : BaseModel
    {
        public Guid Id { get; set; }
        public string SenderId { get; set; }
        public User Sender { get; set; } = null!;
        public string ReceiverId { get; set; }
        public User Receiver { get; set; } = null!;

        public Enums.FriendRequestStatus Status { get; set; }
    }
}
