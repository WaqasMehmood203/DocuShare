using DMS.Backend.Data;
using Ganss.Xss;
using DMS.Backend.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static DMS.Backend.Models.Enums;
using System;
using DMS.Backend.Services;

namespace DMS.Backend.API.Controllers
#region Constructor
{
    public class DocumentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Document Create 
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.VisibilityOptions = Enum.GetValues(typeof(DocumentVisibility))
                .Cast<DocumentVisibility>()
                .Select(v => new SelectListItem
                {
                    Value = ((int)v).ToString(),
                    Text = v.ToString()
                }).ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string title, string content, int visibility, string tags, IFormFile uploadedFile)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            {
                return Unauthorized();
            }
            var sanitizer = new HtmlSanitizer();
            var sanitizedContent = sanitizer.Sanitize(content); // ✨ sanitize content here

            var document = new Document
            {
                Id = Guid.NewGuid(),
                Title = title,
                Content = content,
                Visibility = (DocumentVisibility)visibility,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                OwnerId = userId,
                Tags = tags
            };

            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(uploadedFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(stream);
                }

                document.FilePath = Path.Combine("uploads", uniqueFileName);
            }

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return RedirectToAction("Homepage", "Home");
        }
        public async Task<IActionResult> MyDocuments()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            {
                return Unauthorized();
            }

            var userDocuments = await _context.Documents
                .Where(d => d.OwnerId == userId)
                .ToListAsync();

            return View(userDocuments);
        }
        #endregion

        #region View Document

        [HttpGet]
        public async Task<IActionResult> ViewDocument(Guid id)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            {
                return Unauthorized();
            }

            var document = await _context.Documents
                .Include(d => d.Owner)
                .Include(d => d.Comments)
                    .ThenInclude(c => c.User)
                .Include(d => d.Comments)
                    .ThenInclude(c => c.Replies)
                        .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

            if (document == null)
            {
                return NotFound();
            }

            // Check visibility permissions
            if (document.Visibility == DocumentVisibility.Private && document.OwnerId != userId)
            {
                return Unauthorized("You do not have permission to view this private document.");
            }

            if (document.Visibility == DocumentVisibility.Friends)
            {
                var isFriend = await _context.Friends
                    .AnyAsync(f => (f.UserId == userId && f.FriendId == document.OwnerId) ||
                                  (f.UserId == document.OwnerId && f.FriendId == userId));
                if (!isFriend && document.OwnerId != userId)
                {
                    return Unauthorized("You do not have permission to view this friends-only document.");
                }
            }

            return View(document);
        }
        #endregion

        #region Post Comment
        //[HttpPost]
        [HttpPost]
        public async Task<IActionResult> AddComment(Guid documentId, string content, Guid? parentCommentId = null)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            {
                return Unauthorized();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(d => d.Id == documentId && !d.IsDeleted);
            if (document == null)
            {
                return NotFound();
            }

            // Check visibility permissions
            if (document.Visibility == DocumentVisibility.Private && document.OwnerId != userId)
            {
                return Unauthorized("You do not have permission to comment on this private document.");
            }

            if (document.Visibility == DocumentVisibility.Friends)
            {
                var isFriend = await _context.Friends
                    .AnyAsync(f => (f.UserId == userId && f.FriendId == document.OwnerId) ||
                                  (f.UserId == document.OwnerId && f.FriendId == userId));
                if (!isFriend && document.OwnerId != userId)
                {
                    return Unauthorized("You do not have permission to comment on this friends-only document.");
                }
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                return BadRequest("Comment content cannot be empty.");
            }

            // Validate parent comment if provided
            if (parentCommentId.HasValue)
            {
                var parentComment = await _context.Comments
                    .FirstOrDefaultAsync(c => c.Id == parentCommentId && c.DocumentId == documentId && !c.IsDeleted);
                if (parentComment == null)
                {
                    return BadRequest("Parent comment not found or invalid.");
                }
            }

            var sanitizer = new HtmlSanitizer();
            var sanitizedContent = sanitizer.Sanitize(content);

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DocumentId = documentId,
                Content = sanitizedContent,
                ParentCommentId = parentCommentId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewDocument", new { id = documentId });
        }
        #endregion

        #region Edit Document
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
                return NotFound();

            ViewBag.VisibilityOptions = Enum.GetValues(typeof(DocumentVisibility))
                .Cast<DocumentVisibility>()
                .Select(v => new SelectListItem
                {
                    Value = ((int)v).ToString(),
                    Text = v.ToString()
                }).ToList();

            return View(document);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Document model, IFormFile uploadedFile)
        {
            var document = await _context.Documents.FindAsync(model.Id);
            if (document == null)
                return NotFound();

            document.Title = model.Title;
            document.Content = new Ganss.Xss.HtmlSanitizer().Sanitize(model.Content);
            document.Visibility = model.Visibility;
            document.Tags = model.Tags;
            document.UpdatedDate = DateTime.UtcNow;

            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(uploadedFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(stream);
                }

                document.FilePath = Path.Combine("uploads", uniqueFileName);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("MyDocuments");
        }
        #endregion

        #region Delete document
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
                return NotFound();

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyDocuments");
        }
    }
}
        #endregion
