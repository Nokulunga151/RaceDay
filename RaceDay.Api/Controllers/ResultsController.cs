using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Contracts;
using RaceDay.Api.Models;
using Results = RaceDay.Api.Models.Results;

namespace RaceDay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDBContext _context;

        public ResultsController(RaceDayDBContext context)
        {
            _context = context;
        }

        [HttpGet("mine")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> GetMyResults()
        {
            var participantId = HttpContext.Session.GetInt32("UserId");
            if (participantId == null) return Unauthorized();

            var results = await _context.Results
                .Where(r => r.Enrolment.ParticipantID == participantId)
                .ToListAsync();

            return Ok(results);
        }


        [HttpGet("/api/events/{eventId}/results")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> GetEventResults(string eventId)
        {
            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null) return NotFound();

            var results = await _context.Results
                .Where(r => r.Enrolment.EventID == eventId)
                .ToListAsync();

            return Ok(results);
        }

        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateResult(CreateResultRequest dto)
        {
            var enrolment = await _context.Enrolments
                .Include(e => e.Event)
                .FirstOrDefaultAsync(e => e.EnrolmentID == dto.EnrolmentID);

            if (enrolment == null) return NotFound("Enrolment not found.");

           
            var organiserId = HttpContext.Session.GetInt32("UserId");
            if (organiserId == null || enrolment.Event.OrganiserID != organiserId)
                return Forbid("You can only record results for your own events.");

            var result = new Results
            {
                ResultID = Guid.NewGuid().ToString(),
                EnrolmentID = dto.EnrolmentID,
                FinishTime = dto.FinishTime,
                Position = dto.Position,
                ResultStatus = dto.ResultStatus
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();
            return StatusCode(201, result);

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateResult(string id, UpdateResultRequest dto)
        {
            var result = await _context.Results
                .Include(r => r.Enrolment)
                .ThenInclude(e => e.Event)
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (result == null) return NotFound("Result not found.");

            var organiserId = HttpContext.Session.GetInt32("UserId");
            if (organiserId == null || result.Enrolment.Event.OrganiserID != organiserId)
                return StatusCode(403,"You can only record results for your own events.");

            result.FinishTime = dto.FinishTime ?? result.FinishTime;
            result.Position = dto.Position ?? result.Position;
            result.ResultStatus = dto.ResultStatus ?? result.ResultStatus;

            await _context.SaveChangesAsync();
            return Ok(result);
        }

    }
}
