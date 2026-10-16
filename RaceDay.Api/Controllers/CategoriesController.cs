using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
using System.Data;
using RaceDay.Contracts;

namespace RaceDay.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayDBContext _context;

        public CategoriesController(RaceDayDBContext context)
        {
            _context = context;
        }


        [HttpGet("/api/events/{eventId}/categories")]
        public async Task<IActionResult> GetCategories(string eventId)
        {
            var categories = await _context.Categories
                .Where(c => c.EventID == eventId)
                .ToListAsync();

            if (!categories.Any()) return NotFound("Event not found or no categories");
            return Ok(categories);

        }

        [HttpPost("/api/events/{eventId}/categories")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateCategory(string eventId, CreateCategoryRequest dto)
        {
            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null) return NotFound("Event not found.");

            var category = new Category
            {
                CategoryID = Guid.NewGuid().ToString(),
                CategoryName = dto.CategoryName,
                Distance = dto.Distance,
                EntryFee = dto.EntryFee,
                EventID = eventId
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return StatusCode(201, category);

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateCategory(string id, UpdateCategoryRequest dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            category.CategoryName = dto.CategoryName;
            category.Distance = dto.Distance;
            category.EntryFee = dto.EntryFee;

            await _context.SaveChangesAsync();
            return Ok(category);

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok("Category deleted.");

        }

    }
}
