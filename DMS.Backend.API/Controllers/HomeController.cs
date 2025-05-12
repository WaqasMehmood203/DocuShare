using DMS.Backend.API.Models;
using DMS.Backend.Data;
using DMS.Backend.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static DMS.Backend.Models.Enums;
using DMS.Backend.Models;
namespace DMS.Backend.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
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

                // Fetch documents
                var documents = await _context.Documents
                    .Include(d => d.Owner)
                    .Where(d =>
                        // User's own documents
                        d.OwnerId == userId ||
                        // Public documents from others
                        (d.Visibility == Enums.DocumentVisibility.Public && d.OwnerId != userId) ||
                        // Friends' documents with Friends visibility
                        (d.Visibility == Enums.DocumentVisibility.Friends && friendIds.Contains(d.OwnerId))
                    )
                    .Where(d => !d.IsDeleted)
                    .OrderByDescending(d => d.CreatedDate)
                    .ToListAsync();

                return View(documents);
            }
            catch (Exception ex)
            {
                // Log the error (for now, return a view with the error message)
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View(new List<Document>());
            }
        }

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

        //public IActionResult Homepage()
        //{
        //    if (HttpContext.Session.GetString("UserSession") != null)
        //    {
        //        ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login");
        //    }
        //}
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

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
