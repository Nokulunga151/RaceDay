using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
using RaceDay.Contracts;

namespace RaceDay.Api.Controllers
{
    [ApiController]
    [Route("api/enrolments")]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayDBContext _context;

        public EnrolmentsController(RaceDayDBContext context)
        {
            _context = context;
        }


        [HttpPost]
        
        public async Task<IActionResult> CreateEnrolment(CreateEnrolmentRequest dto)
        {
            var eventId = dto.EventID;

            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null) return NotFound("Event not found.");

            var category = await _context.Categories.FindAsync(dto.CategoryID);
            if (category == null) return NotFound("Category not found.");

            var participantId = HttpContext.Session.GetInt32("UserId");
            if (participantId == null) return Unauthorized();

            var alreadyEnrolled = await _context.Enrolments
                .AnyAsync(e => e.EventID == eventId && e.ParticipantID == participantId);
            if (alreadyEnrolled) return Conflict("Already enrolled.");

            var enrolment = new Enrolment
            {
                EnrolmentID = Guid.NewGuid().ToString(),
                ParticipantID = participantId.Value,
                EventID = eventId,
                CategoryID = dto.CategoryID,
                EnrolmentDate = DateTime.UtcNow,
                EnrolmentStatus = "Pending"
            };

            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();
            return StatusCode(201, enrolment);

        }

        [HttpGet]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> GetMyEnrolments()
        {
            var participantId = HttpContext.Session.GetInt32("UserId");
            if (participantId == null) return Unauthorized();

            var enrolments = await _context.Enrolments
                .Where(e => e.ParticipantID == participantId)
                .ToListAsync();

            return Ok(enrolments);
           
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> CancelEnrolment(string id)
        {
            var enrolment = await _context.Enrolments.FindAsync(id);
            if (enrolment == null) return NotFound();

            _context.Enrolments.Remove(enrolment);
            await _context.SaveChangesAsync();
            return Ok("Enrolment cancelled");
        }

    }
}
