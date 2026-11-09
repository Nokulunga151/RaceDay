using Microsoft.AspNetCore.Mvc;
using RaceDay.Contracts;
using RaceDay.Mvc.Services;
using RaceDay.Mvc.ViewModels;

namespace RaceDay.Mvc.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly RaceDayApiClient _api;

        public ParticipantController(RaceDayApiClient api)
        {
            _api = api;
        }

        private bool IsParticipant()
        {
            return HttpContext.Session.GetString("Role") == "Participant";
        }

        public async Task<IActionResult> Index()
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Auth");

            var events = await _api.GetAsync<List<EventViewModel>>(
                "api/events");

            return View(events ?? new List<EventViewModel>());
        }

        public async Task<IActionResult> MyEvents()
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Auth");

            var enrolments = await _api.GetAsync<List<EnrolmentViewModel>>(
                "api/enrolments");

            return View(enrolments ?? new List<EnrolmentViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> EnterEvent(
            string eventId,
            string categoryId)
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Auth");

            var request = new CreateEnrolmentRequest
            {
                EventID = eventId,
                CategoryID = categoryId
            };

            var response = await _api.PostAsync("api/enrolments", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                TempData["Error"] =
                    $"Could not enter this event. API says: {response.StatusCode} - {error}";
            }
            else
            {
                TempData["Success"] = "You have successfully entered the event.";
            }

            return RedirectToAction("Details", "Events", new { id = eventId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelEnrolment(string id)
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Auth");

            var response = await _api.DeleteAsync($"api/enrolments/{id}");

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Your enrolment has been cancelled.";
            else
                TempData["Error"] = "Unable to cancel this enrolment.";

            return RedirectToAction("MyEvents");
        }



        public async Task<IActionResult> MyResults()
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Auth");

            var results = await _api.GetAsync<List<ResultViewModel>>(
                "api/results/mine");

            return View(results ?? new List<ResultViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Auth");

            var profile = await _api.GetAsync<ProfileViewModel>(
                "api/profile/me");

            if (profile == null)
                return NotFound();

            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(model);

            var request = new UpdateProfileRequest
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                Phone = model.Phone,
                Age = model.Age
            };

            var response = await _api.PutAsync("api/profile/me", request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Could not update your profile.");
                return View(model);
            }

            TempData["Success"] = "Profile updated successfully.";

            return RedirectToAction("Profile");
        }
    }
}
