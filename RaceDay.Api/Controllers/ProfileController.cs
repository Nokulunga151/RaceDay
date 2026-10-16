using Microsoft.AspNetCore.Mvc;
using RaceDay.Api.Data;
using RaceDay.Contracts;

namespace RaceDay.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly RaceDayDBContext _context;

        public ProfileController(RaceDayDBContext context)
        {
            _context = context;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");
            if (userId == null || role == null) return Unauthorized();

            if (role == "Organiser")
            {
                var organiser = await _context.Organisers.FindAsync(userId);
                if (organiser == null) return NotFound();
                return Ok(organiser);
            }
            else
            {
                var participant = await _context.Participants.FindAsync(userId);
                if (participant == null) return NotFound();
                return Ok(participant);
            }
        }


        [HttpPut("me")]
        
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequest dto)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");
            if (userId == null || role == null) return Unauthorized();

            if (role == "Organiser")
            {
                var organiser = await _context.Organisers.FindAsync(userId);
                if (organiser == null) return NotFound();

                organiser.FirstName = dto.Name;
                organiser.LastName = dto.Surname;
                organiser.Email = dto.Email;
                organiser.Phone = dto.Phone;

                await _context.SaveChangesAsync();
                return Ok(organiser);
            }
            else
            {
                var participant = await _context.Participants.FindAsync(userId);
                if (participant == null) return NotFound();

                participant.Name = dto.Name;
                participant.Surname = dto.Surname;
                participant.Email = dto.Email;
                participant.Phone = dto.Phone;
                participant.Age = dto.Age ?? participant.Age;

                await _context.SaveChangesAsync();
                return Ok(participant);
            }

        }
    }
}
