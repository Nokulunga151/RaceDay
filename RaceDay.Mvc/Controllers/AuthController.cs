using Microsoft.AspNetCore.Mvc;
using RaceDay.Mvc.Services;
using RaceDay.Contracts;
using RaceDay.Mvc.ViewModels;
using System.Text.Json;

namespace RaceDay.Mvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly RaceDayApiClient _api;

        public AuthController(RaceDayApiClient api)
        {
            _api = api;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var request = new LoginRequest
            {
                Email = model.Email,
                Password = model.Password
            };

            var response = await _api.PostAsync("api/auth/login", request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            var json = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(json);

            var root = document.RootElement;

            int userId = root.GetProperty("userId").GetInt32();
            string role = root.GetProperty("role").GetString() ?? "";

            HttpContext.Session.SetInt32("UserId", userId);
            HttpContext.Session.SetString("Role", role);

            if (role == "Organiser")
                return RedirectToAction("Index", "Organiser");

            return RedirectToAction("Index", "Participant");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var request = new RegisterRequest
            {
                Role = model.Role,
                Email = model.Email,
                Password = model.Password,
                Phone = model.Phone,

                // Organiser details
                FirstName = model.FirstName,
                LastName = model.LastName,

                // Participant details
                Name = model.FirstName,
                Surname = model.LastName,
                Age = model.Age
            };

            var response = await _api.PostAsync("api/auth/register", request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Registration failed. Please check your details.");
                return View(model);
            }

            TempData["Success"] = "Registration successful. Please log in.";

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _api.PostAsync("api/auth/logout", new { });

            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}
