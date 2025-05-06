using DMS.Backend.API.Models;
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

        [HttpPost]
        public async Task<IActionResult> CreateDocument(DocumentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var document = new DocumentViewModel
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Content = model.Content,
                OwnerId = Guid.Parse(User.Identity.Name),
                Visibility = model.Visibility,
                Tags = model.Tags,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = null,
                IsUpdated = false
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return Ok(new { Id = document.Id });
        }
    }
}
