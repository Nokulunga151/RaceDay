using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
using RaceDay.Contracts;


namespace RaceDay.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayDBContext _context;
        private readonly PasswordHasher<string> _passwordHasher = new();

        public AuthController(RaceDayDBContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Email and password are required");
            }

            var email = dto.Email.Trim().ToLower();

            if (await _context.Organisers.AnyAsync(o => o.Email == email) ||
                await _context.Participants.AnyAsync(p => p.Email == email))
            {
                return Conflict("Email already registered");
            }


            if (dto.Role == "Organiser")
            {
                if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                    string.IsNullOrWhiteSpace(dto.LastName))
                {
                    return BadRequest(
                        "Organisers First Name and Last Name are required.");
                }

                var organiser = new Organiser
                {
                    FirstName = dto.FirstName.Trim(),
                    LastName = dto.LastName.Trim(),
                    Email = email,
                    Phone = dto.Phone,
                    PasswordHash = _passwordHasher.HashPassword(email, dto.Password)
                };

                _context.Organisers.Add(organiser);
            }
            else if (dto.Role == "Participant")
            {
                if (string.IsNullOrWhiteSpace(dto.Name) ||
                    string.IsNullOrWhiteSpace(dto.Surname) ||
                    dto.Age is null || dto.Age <= 0)
                {
                    return BadRequest(
                        "Participants Name, Surname and valid Age are required.");
                }

                var participant = new Participants
                {
                    Name = dto.Name.Trim(),
                    Surname = dto.Surname.Trim(),
                    Email = email,
                    Phone = dto.Phone,
                    Age = dto.Age.Value,
                    PasswordHash = _passwordHasher.HashPassword(email, dto.Password)
                };

                _context.Participants.Add(participant);
            }
            else
            {
                return BadRequest(
                    "Role must be Organiser or Participant.");
            }

            await _context.SaveChangesAsync();

            return StatusCode(201, "Account successfully created.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var email = dto.Email.Trim().ToLower();

            var organiser = await _context.Organisers.FirstOrDefaultAsync(o => o.Email == email);

            if (organiser != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(email, organiser.PasswordHash, dto.Password);

                if (result == PasswordVerificationResult.Failed)
                    return Unauthorized("Invalid Email or Password");

                HttpContext.Session.SetInt32("UserId", organiser.OrganiserID);
                HttpContext.Session.SetString("Role", "Organiser");

                return Ok(new
                {
                    message = "Login successful",
                    role = "Organiser",
                    userId = organiser.OrganiserID
                });
            }

            var participant = await _context.Participants.FirstOrDefaultAsync(p => p.Email == email);

            if(participant != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(email, participant.PasswordHash, dto.Password);

                if (result == PasswordVerificationResult.Failed)
                    return Unauthorized("Invalid Email or Password");

                HttpContext.Session.SetInt32("UserId", participant.ParticipantID);
                HttpContext.Session.SetString("Role", "Participant");

                return Ok(new
                {
                    message = "Login successful.",
                    role = "Participant",
                    userId = participant.ParticipantID
                });
            }

            return Unauthorized("Invalid Email or Password");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok("Logged out successfully.");
        }
    }
}
