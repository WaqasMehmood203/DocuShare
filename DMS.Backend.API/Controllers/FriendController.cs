using DMS.Backend.Data;
using DMS.Backend.Models.Entities;
using DMS.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMS.Backend.API.Controllers
{
    public class FriendController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FriendController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Index
        public async Task<IActionResult> Index(string searchString)
        {
            var userId = GetCurrentUserId();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var searchTerms = searchString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                IQueryable<User> users = _context.Users.Where(u => u.Id != userId);

                if (searchTerms.Length == 1)
                {
                    string term = searchTerms[0];
                    users = users.Where(u => u.FirstName.Contains(term) || u.LastName.Contains(term));
                }
                else if (searchTerms.Length >= 2)
                {
                    string firstName = searchTerms[0];
                    string lastName = searchTerms[1];
                    users = users.Where(u => u.FirstName.Contains(firstName) && u.LastName.Contains(lastName));
                }

                var searchResults = await users.ToListAsync();
                ViewBag.SearchResults = searchResults;

                var friendIds = await _context.Friends
                    .Where(f => f.UserId == userId || f.FriendId == userId)
                    .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
                    .Distinct()
                    .ToListAsync();
                ViewBag.FriendIds = friendIds;
            }
            else
            {
                var friendIds = await _context.Friends
                    .Where(f => f.UserId == userId)
                    .Select(f => f.FriendId)
                    .Distinct()
                    .ToListAsync();
                var friends = await _context.Users
                    .Where(u => friendIds.Contains(u.Id))
                    .ToListAsync();
                ViewBag.Friends = friends;
            }

            return View();
        }
        #endregion

        #region Send Friend Request
        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(Guid receiverId) 
        {
            var userId = GetCurrentUserId();
            var receiver = await _context.Users.FindAsync(receiverId);
            if (receiver == null)
            {
                return Content("User not found.");
            }
            var isFriends = await _context.Friends.AnyAsync(f => (f.UserId == userId && f.FriendId == receiverId) || (f.UserId == receiverId && f.FriendId == userId));
            if (isFriends)
            {
                return Content("Already friends.");
            }
            var existingRequest = await _context.FriendRequests.FirstOrDefaultAsync(fr => fr.SenderId == userId && fr.ReceiverId == receiverId && fr.Status == Enums.FriendRequestStatus.Pending);
            if (existingRequest != null)
            {
                return Content("Friend request already sent.");
            }
            var friendRequest = new FriendRequest   
            {
                SenderId = userId,
                ReceiverId = receiverId,
                Status = Enums.FriendRequestStatus.Pending
            };
            _context.FriendRequests.Add(friendRequest);
            var sender = await _context.Users.FindAsync(userId);
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                ReceiverId = receiverId,
                Message = $"{sender.FirstName} {sender.LastName} sent you a friend request",
                Type = Enums.NotificationType.FriendRequest,
                IsRead = false,
                CreatedDate = DateTime.UtcNow
            };
            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private Guid GetCurrentUserId()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return Guid.Empty;
            }
            return Guid.Parse(userIdString);
        }
        #endregion
    }
}
