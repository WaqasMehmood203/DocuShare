using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Backend.Models
{
    public static class Enums
    {
        public enum DocumentVisibility
        {
            Public = 0,
            Private = 1,
            Friends = 2
        }
        public enum FriendRequestStatus
        {
            Pending,
            Accepted,
            Declined
        }

        public enum NotificationType
        {
            DocumentShared,
            FriendRequest,
            DocumentLiked,
            DocumentCommented
        }

        public enum Gender
        {
            Male,
            Female,
            Other
        }
    }
}
