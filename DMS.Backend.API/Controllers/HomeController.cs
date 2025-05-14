using DMS.Backend.API.Models;
using DMS.Backend.Data;
using DMS.Backend.Models;
using DMS.Backend.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Backend.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Index

        public async Task<IActionResult> Index(string searchString, string visibilityFilter, int page = 1, string sortBy = "CreatedDate", string sortOrder = "desc")
        {
            try
            {
                var userIdStr = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                {
                    return RedirectToAction("Login");
                }

                // Fetch friends' IDs
                var friendIds = await _context.Friends
                    .Where(f => f.UserId == userId || f.FriendId == userId)
                    .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
                    .Distinct()
                    .ToListAsync();

                // Base query for documents
                var query = _context.Documents
                    .Include(d => d.Owner)
                    .Where(d =>
                        // User's own documents
                        d.OwnerId == userId ||
                        // Public documents from others
                        (d.Visibility == Enums.DocumentVisibility.Public && d.OwnerId != userId) ||
                        // Friends' documents with Friends visibility
                        (d.Visibility == Enums.DocumentVisibility.Friends && friendIds.Contains(d.OwnerId))
                    )
                    .Where(d => !d.IsDeleted);

                // Apply search filter
                if (!string.IsNullOrEmpty(searchString))
                {
                    query = query.Where(d => d.Title.Contains(searchString));
                }

                // Apply visibility filter
                if (!string.IsNullOrEmpty(visibilityFilter) && Enum.TryParse<Enums.DocumentVisibility>(visibilityFilter, out var visibility))
                {
                    query = query.Where(d => d.Visibility == visibility);
                }

                // Apply sorting
                switch (sortBy.ToLower())
                {
                    case "title":
                        query = sortOrder.ToLower() == "asc" ? query.OrderBy(d => d.Title) : query.OrderByDescending(d => d.Title);
                        break;
                    case "owner":
                        query = sortOrder.ToLower() == "asc"
                            ? query.OrderBy(d => d.Owner.FirstName).ThenBy(d => d.Owner.LastName)
                            : query.OrderByDescending(d => d.Owner.FirstName).ThenByDescending(d => d.Owner.LastName);
                        break;
                    case "visibility":
                        query = sortOrder.ToLower() == "asc" ? query.OrderBy(d => d.Visibility) : query.OrderByDescending(d => d.Visibility);
                        break;
                    case "createddate":
                    default:
                        query = sortOrder.ToLower() == "asc" ? query.OrderBy(d => d.CreatedDate) : query.OrderByDescending(d => d.CreatedDate);
                        break;
                }

                // Get total count for pagination
                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

                // Ensure page is within valid range
                page = Math.Max(1, Math.Min(page, totalPages));

                // Fetch paginated documents
                var documents = await query
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                // Pass pagination and sorting data to view
                ViewBag.SearchString = searchString;
                ViewBag.VisibilityFilter = visibilityFilter;
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.SortBy = sortBy;
                ViewBag.SortOrder = sortOrder;
                ViewBag.NextSortOrder = sortOrder.ToLower() == "asc" ? "desc" : "asc";

                return View(documents);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View(new List<Document>());
            }
        }

        #endregion

        #region Login and Logout
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // In HomeController.cs
        [HttpPost]
        public IActionResult Login(User user)
        {
            var myUser = _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
            if (myUser != null)
            {
                HttpContext.Session.SetString("UserSession", myUser.Email);
                HttpContext.Session.SetString("UserId", myUser.Id.ToString());

                var notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    ReceiverId = myUser.Id,
                    Message = "You have successfully logged in.",
                    Type = Enums.NotificationType.LoginSuccessful,
                    IsRead = false,
                    CreatedDate = DateTime.UtcNow
                };
                _context.Notifications.Add(notification);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Login Failed.";
                return View();
            }
        }
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                HttpContext.Session.Remove("UserId");
                return RedirectToAction("Login");
            }
            return View();
        }
        #endregion

        #region Homepage
        public IActionResult Homepage()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

#endregion

        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        #region Register
        //        public IActionResult Register()
        //        {
        //            return View();
        //        
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError(nameof(user.Email), "That email is already registered.");
                return View(user);
            }

            if (!ModelState.IsValid)
            {

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Registered Successfully";
                return RedirectToAction("Login");
            }

            return View();
        }
        #endregion

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
#endregion