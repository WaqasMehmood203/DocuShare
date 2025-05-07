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
                // Update status to Accepted
                request.Status = Enums.FriendRequestStatus.Accepted;
                // Create mutual friendships
                var friend = new Friend { UserId = userId, FriendId = request.SenderId };
                var friend2 = new Friend { UserId = request.SenderId, FriendId = userId };
                _context.Friends.Add(friend);
                _context.Friends.Add(friend2);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
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

        private Guid GetCurrentUserId()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Guid.Empty;
            }
            return userId;
        }
    }
}
