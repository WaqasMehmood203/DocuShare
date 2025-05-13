using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DMS.Backend.Models.Enums;

namespace DMS.Backend.Models.Entities
{
    public class Friend 
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid FriendId { get; set; }
        public User FriendUser { get; set; } = null!;
        public FriendRequestStatus Status { get; set; }

    }
}
