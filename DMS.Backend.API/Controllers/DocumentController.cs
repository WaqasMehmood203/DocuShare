using DMS.Backend.API.Models.Entities;
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

        [HttpPost]
        public async Task<IActionResult> Create(string title, string content, int visibility, string tags)
        {
            //var userId = Guid.Parse(User.FindFirstValue("UserId")); // Make sure UserId claim exists
            var userId = Guid.Parse("FFE63318-D0DC-41A6-D567-08DD8A4BD201"); // Hardcoded for now

            var document = new Document
            {
                Id = Guid.NewGuid(),
                Title = title,
                Content = content,
                OwnerId = userId,
                Visibility = (DocumentVisibility)visibility,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate  = DateTime.UtcNow,
                DocumentTags = new List<DocumentTag>()
            };

            // Handle tags
            var tagNames = tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                               .Select(t => t.Trim().ToLower())
                               .Distinct();

            foreach (var tagName in tagNames)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag { Id = Guid.NewGuid(), Name = tagName };
                    _context.Tags.Add(tag);
                }

                document.DocumentTags.Add(new DocumentTag
                {
                    DocumentId = document.Id,
                    TagId = tag.Id
                });
            }

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
