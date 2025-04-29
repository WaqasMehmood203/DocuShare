using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Backend.Models.Entities
{
    public class Friend 
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; } = null!;
        public string FriendId { get; set; }
        public User FriendUser { get; set; } = null!;
    }
}
