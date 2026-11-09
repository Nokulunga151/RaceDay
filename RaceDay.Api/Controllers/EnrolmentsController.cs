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
            var participantId = HttpContext.Session.GetInt32("UserId");

            if (participantId == null)
                return Unauthorized();

            var ev = await _context.Events.FindAsync(dto.EventID);

            if (ev == null)
                return NotFound("Event not found.");

            var category = await _context.Categories.FindAsync(dto.CategoryID);

            if (category == null || category.EventID != dto.EventID)
                return NotFound("Category not found for this event.");

            var alreadyEnrolled = await _context.Enrolments
                .AnyAsync(e => e.EventID == dto.EventID
                    && e.ParticipantID == participantId.Value);

            if (alreadyEnrolled)
                return Conflict("Already enrolled.");

            var enrolment = new Enrolment
            {
                EnrolmentID = Guid.NewGuid().ToString(),
                ParticipantID = participantId.Value,
                EventID = dto.EventID,
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
                .Where(e => e.ParticipantID == participantId.Value)
                .ToListAsync();

            return Ok(enrolments);
           
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> CancelEnrolment(string id)
        {
            var participantId = HttpContext.Session.GetInt32("UserId");

            if (participantId == null)
                return Unauthorized();

            var enrolment = await _context.Enrolments
                .Include(e => e.Result)
                .FirstOrDefaultAsync(e => e.EnrolmentID == id);

            if (enrolment == null)
                return NotFound("Enrolment not found.");

            if (enrolment.ParticipantID != participantId.Value)
                return Forbid();

            if (enrolment.Result != null)
                return Conflict("This enrolment cannot be cancelled because a result has already been recorded.");

            _context.Enrolments.Remove(enrolment);
            await _context.SaveChangesAsync();

            return Ok("Enrolment cancelled successfully.");
        }



        [HttpGet("event/{eventId}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> GetEventEnrolments(string eventId)
        {
            var organiserId = HttpContext.Session.GetInt32("UserId");

            if (organiserId == null)
                return Unauthorized();

            var ev = await _context.Events.FindAsync(eventId);

            if (ev == null)
                return NotFound("Event not found.");

            if (ev.OrganiserID != organiserId.Value)
                return Forbid();

            var enrolments = await _context.Enrolments
                .Where(e => e.EventID == eventId)
                .Include(e => e.Participant)
                .Select(e => new
                {
                    e.EnrolmentID,
                    e.EventID,
                    e.CategoryID,
                    e.EnrolmentDate,
                    e.EnrolmentStatus,
                    e.ParticipantID,
                    ParticipantName = e.Participant.Name,
                    ParticipantSurname = e.Participant.Surname,
                    ParticipantEmail = e.Participant.Email,
                    ParticipantPhone = e.Participant.Phone
                })
                .ToListAsync();

            return Ok(enrolments);
        }


    }
}
