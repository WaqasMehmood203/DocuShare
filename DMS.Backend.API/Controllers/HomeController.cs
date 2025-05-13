using DMS.Backend.API.Models;
using DMS.Backend.Data;
using DMS.Backend.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static DMS.Backend.Models.Enums;
using DMS.Backend.Models;
using DMS.Backend.Models.Entities;
namespace DMS.Backend.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        #region Index
        //public async Task<IActionResult> Index()
        //{
        //    try
        //    {
        //        var userIdStr = HttpContext.Session.GetString("UserId");
        //        if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
        //        {
        //            return RedirectToAction("Login");
        //        }

            //        var friendIds = await _context.Friends
            //            .Where(f => f.UserId == userId || f.FriendId == userId)
            //            .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
            //            .Distinct()
            //            .ToListAsync();

            //        var documents = await _context.Documents
            //            .Include(d => d.Owner)
            //            .Where(d =>
            //                d.OwnerId == userId ||
            //                (d.Visibility == Enums.DocumentVisibility.Public && d.OwnerId != userId) ||
            //                (d.Visibility == Enums.DocumentVisibility.Friends && friendIds.Contains(d.OwnerId))
            //            )
            //            .Where(d => !d.IsDeleted)
            //            .OrderByDescending(d => d.CreatedDate)
            //            .ToListAsync();

            //        return View(documents);
            //    }
            //    catch (Exception ex)
            //    {
            //        ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
            //        return View(new List<Document>());
            //    }
            //}
        public async Task<IActionResult> Index([FromQuery] QueryParameters parameters)
        {
            // Get current user ID from session
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            {
                // If not logged in, show only public documents
                userId = Guid.Empty;
            }

            // Base query: Documents owned by user or shared with user
            var query = _context.Documents
                .Include(d => d.Owner)
                .AsNoTracking()
                .Where(d => d.OwnerId == userId ||
                           d.DocumentShares.Any(s => s.UserId == userId) ||
                           d.Visibility == DocumentVisibility.Public)
                .AsQueryable();

            // Apply visibility filter
            if (!string.IsNullOrEmpty(parameters.Visibility))
            {
                if (parameters.Visibility == "Public")
                {
                    query = query.Where(d => d.Visibility == DocumentVisibility.Public);
                }
                else if (parameters.Visibility == "Private" && userId != Guid.Empty)
                {
                    query = query.Where(d => d.Visibility == DocumentVisibility.Private && d.OwnerId == userId);
                }
                else if (parameters.Visibility == "Friends" && userId != Guid.Empty)
                {
                    query = query.Where(d => d.Visibility == DocumentVisibility.Friends &&
                        (d.OwnerId == userId || _context.Friends.Any(f =>
                            f.UserId == userId && f.FriendId == d.OwnerId && f.Status == FriendRequestStatus.Accepted)));
                }
            }

            // Apply tag filter
            if (!string.IsNullOrEmpty(parameters.Tags))
            {
                var tags = parameters.Tags.Split(',').Select(t => t.Trim().ToLower());
                query = query.Where(d => d.Tags != null && tags.Any(t => d.Tags.ToLower().Contains(t)));
            }

            // Apply search
            if (!string.IsNullOrEmpty(parameters.SearchString))
            {
                var search = parameters.SearchString.ToLower();
                query = query.Where(d => d.Title.ToLower().Contains(search) ||
                                       d.Content.ToLower().Contains(search));
            }

            // Apply sorting
            query = parameters.SortBy.ToLower() switch
            {
                "title" => parameters.SortOrder == "asc"
                    ? query.OrderBy(d => d.Title)
                    : query.OrderByDescending(d => d.Title),
                "createddate" => parameters.SortOrder == "asc"
                    ? query.OrderBy(d => d.CreatedDate)
                    : query.OrderByDescending(d => d.CreatedDate),
                _ => query.OrderByDescending(d => d.CreatedDate) // Default
            };

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.ValidPageSize)
                .Take(parameters.ValidPageSize)
                .ToListAsync();

            // Create paged result
            var result = new PagedResult<Document>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.ValidPageSize
            };

            // Pass parameters to view for form persistence
            ViewData["QueryParameters"] = parameters;
            return View(result);
        }

        //public IActionResult Logout()
        //{
        //    HttpContext.Session.Clear();
        //    return RedirectToAction("Login", "Home");
        //}
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

        [HttpPost]
        public IActionResult Login(User user)
        {
            var myUser = _context.Users
       .Where(x => x.Email == user.Email && x.Password == user.Password)
       .FirstOrDefault();

            if (myUser != null)
            {
                HttpContext.Session.SetString("UserSession", myUser.Email);
                HttpContext.Session.SetString("UserId", myUser.Id.ToString()); // Store User ID
                //return RedirectToAction("Homepage");
                return RedirectToAction("Index", "Friend");


            }
            else
            {
                ViewBag.Message = "Login Failed..";
                return View();
            }
        }

       
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }

            return View();
        }
        #endregion

        #region Register
        public IActionResult Register()
        {
            return View();
        }

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

        #region HomePage 
        public async Task<IActionResult> Homepage()
        {
            var userEmail = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userEmail))
                return RedirectToAction("Login");

            var currentUser = await _context.Users
                .Include(u => u.Friends)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            var friendIds = currentUser.Friends
                .Select(f => f.FriendId)
                .ToList();

            var userId = currentUser.Id;

            var documents = await _context.Documents
                .Include(d => d.Owner)
                .Where(d =>
                    d.OwnerId == userId // own documents
                    || d.Visibility == DocumentVisibility.Public // public docs
                    || (d.Visibility == DocumentVisibility.Friends && friendIds.Contains(d.OwnerId)) // friends' docs visible to friends
                )
                .ToListAsync();

            return View(documents);
        }
        #endregion

        #region Privacy
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