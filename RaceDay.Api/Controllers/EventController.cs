
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
using RaceDay.Contracts;

namespace RaceDay.Api.Controllers
{

    [ApiController]
    [Route("api/events")]
    public class EventController : ControllerBase
    {
        private readonly RaceDayDBContext _context;

        public EventController(RaceDayDBContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events
                .Select(e => new
                {
                    e.EventID,
                    e.EventName,
                    e.EventDate,
                    e.Location,
                    e.Description,
                    e.Distance,
                    e.EventType,
                    e.OrganiserID
                })
                .ToListAsync();

            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(string id)
        {
            var eventItem = await _context.Events
                .Where(e => e.EventID == id)
                .Select(e => new
                {
                    e.EventID,
                    e.EventName,
                    e.EventDate,
                    e.Location,
                    e.Description,
                    e.Distance,
                    e.EventType,
                    e.OrganiserID
                })
                .FirstOrDefaultAsync();

            if (eventItem == null)
                return NotFound("Event not found");

            return Ok(eventItem);
        }

        [HttpGet("mine")]
        public async Task<IActionResult> GetMyEvents()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null)
                return Unauthorized();

            if (role != "Organiser")
                return Forbid();

            var events = await _context.Events
                .Where(e => e.OrganiserID == userId.Value)
                .ToListAsync();

            return Ok(events);

        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent (CreateEventRequest dto)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null)
                return Unauthorized();

            if (role != "Organiser")
                return Forbid();

            if (string.IsNullOrWhiteSpace(dto.EventID) ||
                string.IsNullOrWhiteSpace(dto.EventName) ||
                string.IsNullOrWhiteSpace(dto.Location) ||
                string.IsNullOrWhiteSpace(dto.EventType) ||
                dto.Distance <= 0)
            {
                return BadRequest("Please provide valid event details.");
            }

            if (await _context.Events.AnyAsync(
                e => e.EventID == dto.EventID))
            {
                return Conflict("An event with this ID already exists.");
            }

            var newEvent = new Event
            {
                EventID = dto.EventID,
                EventName = dto.EventName.Trim(),
                EventDate = dto.EventDate,
                Location = dto.Location.Trim(),
                Description = dto.Description,
                Distance = dto.Distance,
                EventType = dto.EventType.Trim(),
                OrganiserID = userId.Value
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = newEvent.EventID },
                newEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvents(string id, UpdateEventRequest dto)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null)
                return Unauthorized();

            if (role != "Organiser")
                return Forbid();

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (eventItem == null)
                return NotFound("Event not found.");

            if (eventItem.OrganiserID != userId.Value)
                return Forbid();

            if (string.IsNullOrWhiteSpace(dto.EventName) ||
                string.IsNullOrWhiteSpace(dto.Location) ||
                string.IsNullOrWhiteSpace(dto.EventType) ||
                dto.Distance <= 0)
            {
                return BadRequest("Please provide valid event details.");
            }

            eventItem.EventName = dto.EventName.Trim();
            eventItem.EventDate = dto.EventDate;
            eventItem.Location = dto.Location.Trim();
            eventItem.Description = dto.Description;
            eventItem.Distance = dto.Distance;
            eventItem.EventType = dto.EventType.Trim();

            await _context.SaveChangesAsync();

            return Ok(eventItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            var userId = HttpContext.Session.GetInt32("userId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null)
                return Unauthorized();

            if (role != "Organiser")
                return Forbid();

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (eventItem == null)
                return NotFound("Event not found");

            if (eventItem.OrganiserID != userId.Value)
                return Forbid();

            var hasEnrolments = await _context.Enrolments
                .AnyAsync(en => en.EventID == id);

            if (hasEnrolments)
            {
                return Conflict("This event has enrolments and cannot be deleted");
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
