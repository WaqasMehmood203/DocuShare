using DMS.Backend.Data;
using DMS.Backend.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static DMS.Backend.Models.Enums;

namespace DMS.Backend.API.Controllers
{
    public class DocumentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentController(ApplicationDbContext context)
        {
            _context = context;
        }

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
        //public IActionResult MyDocuments()
        //{
        //    return View();
        //}
        #region Uplaod Function
        //[HttpPost]
        //public async Task<IActionResult> Create(string title, string content, int visibility, string tags)
        //{

        //        var userIdStr = HttpContext.Session.GetString("UserId");
        //        if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
        //        {
        //            return Unauthorized(); // Not logged in
        //        }

        //        var document = new Document
        //        {
        //            Id = Guid.NewGuid(),
        //            Title = title,
        //            Content = content,
        //            Visibility = (DocumentVisibility)visibility,
        //            CreatedDate = DateTime.UtcNow,
        //            UpdatedDate = DateTime.UtcNow,
        //            OwnerId = userId, 
        //            DocumentTags = new List<DocumentTag>()
        //        };

        //        var tagNames = tags.Split(',')
        //                           .Select(t => t.Trim())
        //                           .Where(t => !string.IsNullOrEmpty(t))
        //                           .ToList();

        //        foreach (var tagName in tagNames)
        //        {
        //            var existingTag = await _context.Tags
        //                .FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());

        //            if (existingTag == null)
        //            {
        //                existingTag = new Tag
        //                {
        //                    Id = Guid.NewGuid(),
        //                    Name = tagName
        //                };

        //                _context.Tags.Add(existingTag);
        //            }

        //            document.DocumentTags.Add(new DocumentTag
        //            {
        //                DocumentId = document.Id,
        //                Tag = existingTag
        //            });
        //        }

        //        _context.Documents.Add(document);
        //        await _context.SaveChangesAsync();

        //    return RedirectToAction("Index", "Home");

        //    //catch (Exception ex)
        //    //{
        //    //    return StatusCode(500, new { error = "An error occurred", details = ex.Message });
        //    //}
        //}

        //[HttpPost]
        //        public async Task<IActionResult> Create(string title, string content, int visibility, string tags, IFormFile uploadedFile)
        //        {
        //            var userIdStr = HttpContext.Session.GetString("UserId");
        //            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
        //            {
        //                return Unauthorized();
        //            }

        //            var document = new Document
        //            {
        //                Id = Guid.NewGuid(),
        //                Title = title,
        //                Content = content,
        //                Visibility = (DocumentVisibility)visibility,
        //                CreatedDate = DateTime.UtcNow,
        //                UpdatedDate = DateTime.UtcNow,
        //                OwnerId = userId,
        //                DocumentTags = new List<DocumentTag>()
        //            };

        //            // ✅ Handle file upload
        //            if (uploadedFile != null && uploadedFile.Length > 0)
        //            {
        //                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        //                if (!Directory.Exists(uploadsFolder))
        //                {
        //                    Directory.CreateDirectory(uploadsFolder);
        //                }

        //                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(uploadedFile.FileName)}";
        //                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await uploadedFile.CopyToAsync(stream);
        //                }

        //                // Save relative path or just file name in DB
        //                document.FilePath = Path.Combine("uploads", uniqueFileName);
        //            }

        //            // ✅ Process tags
        //            var tagNames = tags.Split(',')
        //                               .Select(t => t.Trim())
        //                               .Where(t => !string.IsNullOrEmpty(t))
        //                               .ToList();

        //            foreach (var tagName in tagNames)
        //            {
        //                var existingTag = await _context.Tags
        //                    .FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());

        //                if (existingTag == null)
        //                {
        //                    existingTag = new Tag
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        Name = tagName
        //                    };

        //                    _context.Tags.Add(existingTag);
        //                }

        //                document.DocumentTags.Add(new DocumentTag
        //                {
        //                    DocumentId = document.Id,
        //                    Tag = existingTag
        //                });
        //            }

        //            _context.Documents.Add(document);
        //            await _context.SaveChangesAsync();

        //            return RedirectToAction("myDocuments");
        //        }
        //        [HttpGet]
        //        public async Task<IActionResult> MyDocuments()
        //        {
        //            var userIdStr = HttpContext.Session.GetString("UserId");
        //            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
        //            {
        //                return Unauthorized();
        //            }

        //            var documents = await _context.Documents
        //                  .Where(d => d.OwnerId == userId)
        //    .Include(d => d.Owner)
        //    .Include(d => d.DocumentTags)
        //        .ThenInclude(dt => dt.Tag)
        //    .ToListAsync();

        //            return View(documents);
        //        }

        //    }
        //}
        #endregion

        [HttpPost]
        public async Task<IActionResult> Create(string title, string content, int visibility, string tags, IFormFile uploadedFile)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            {
                return Unauthorized();
            }

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

            return RedirectToAction("Index", "Home");
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

    }
}
