using DMS.Backend.Data;
using DMS.Backend.Models;
using DMS.Backend.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMS.Backend.API.Controllers
{
    public class FriendRequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FriendRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                return RedirectToAction("Login", "Home");
            }
            var receivedRequests = await _context.FriendRequests
                .Where(fr => fr.ReceiverId == userId && fr.Status == Enums.FriendRequestStatus.Pending)
                .Include(fr => fr.Sender)
                .ToListAsync();
            return View(receivedRequests);
        }

        // In FriendRequestController.cs
        [HttpPost]
        public async Task<IActionResult> Accept(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                return RedirectToAction("Login", "Home");
            }
            var request = await _context.FriendRequests.FindAsync(id);
            if (request != null && request.ReceiverId == userId && request.Status == Enums.FriendRequestStatus.Pending)
            {
                request.Status = Enums.FriendRequestStatus.Accepted;
                var friend = new Friend { UserId = userId, FriendId = request.SenderId };
                var friend2 = new Friend { UserId = request.SenderId, FriendId = userId };
                _context.Friends.Add(friend);
                _context.Friends.Add(friend2);

                var sender = await _context.Users.FindAsync(request.SenderId);
                var acceptor = await _context.Users.FindAsync(userId);

                var notificationForSender = new Notification
                {
                    Id = Guid.NewGuid(),
                    ReceiverId = request.SenderId,
                    Message = $"{acceptor.FirstName} {acceptor.LastName} accepted your friend request",
                    Type = Enums.NotificationType.FriendRequest,
                    IsRead = false,
                    CreatedDate = DateTime.UtcNow
                };
                _context.Notifications.Add(notificationForSender);

                var notificationForAcceptor = new Notification
                {
                    Id = Guid.NewGuid(),
                    ReceiverId = userId,
                    Message = $"You are now friends with {sender.FirstName} {sender.LastName}",
                    Type = Enums.NotificationType.FriendRequest,
                    IsRead = false,
                    CreatedDate = DateTime.UtcNow
                };
                _context.Notifications.Add(notificationForAcceptor);

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        private Guid GetCurrentUserId()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Guid.Empty;
            }
            return userId;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                return RedirectToAction("Login", "Home");
            }
            var request = await _context.FriendRequests.FindAsync(id);
            if (request != null && request.ReceiverId == userId && request.Status == Enums.FriendRequestStatus.Pending)
            {
                // Set status to Declined
                request.Status = Enums.FriendRequestStatus.Declined;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}
