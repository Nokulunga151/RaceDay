using Microsoft.AspNetCore.Mvc;
using RaceDay.Contracts;
using RaceDay.Mvc.Services;
using RaceDay.Mvc.ViewModels;

namespace RaceDay.Mvc.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly RaceDayApiClient _api;

        public OrganiserController(RaceDayApiClient api)
        {
            _api = api;
        }

        private bool IsOrganiser()
        {
            return HttpContext.Session.GetString("Role") == "Organiser";
        }

        public async Task<IActionResult> Index()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var events = await _api.GetAsync<List<EventViewModel>>(
                "api/events/mine");

            return View(events ?? new List<EventViewModel>());
        }

        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventFormViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(model);

            var request = new CreateEventRequest
            {
                EventID = model.EventID,
                EventName = model.EventName,
                EventDate = model.EventDate,
                Location = model.Location,
                Description = model.Description,
                Distance = model.Distance,
                EventType = model.EventType
            };

            var response = await _api.PostAsync("api/events", request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Could not create the event.");
                return View(model);
            }

            // Upload the selected file after the event has been created.
            if (model.EventFile != null && model.EventFile.Length > 0)
            {
                var uploadResponse = await _api.UploadFileAsync(
                    $"api/events/{Uri.EscapeDataString(model.EventID)}/file",
                    model.EventFile);

                if (!uploadResponse.IsSuccessStatusCode)
                {
                    TempData["Error"] =
                        "Event created, but the file upload failed.";
                    return RedirectToAction("Index");
                }
            }

            TempData["Success"] = "Event created successfully.";

            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> EditEvent(string id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var raceEvent = await _api.GetAsync<EventViewModel>(
                $"api/events/{Uri.EscapeDataString(id)}");

            if (raceEvent == null)
                return NotFound();

            var model = new EventFormViewModel
            {
                EventID = raceEvent.EventID,
                EventName = raceEvent.EventName,
                EventDate = raceEvent.EventDate,
                Location = raceEvent.Location,
                Description = raceEvent.Description,
                Distance = raceEvent.Distance,
                EventType = raceEvent.EventType
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent( string id, EventFormViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(model);

            var request = new UpdateEventRequest
            {
                EventName = model.EventName,
                EventDate = model.EventDate,
                Location = model.Location,
                Description = model.Description,
                Distance = model.Distance,
                EventType = model.EventType
            };

            var response = await _api.PutAsync(
                $"api/events/{Uri.EscapeDataString(id)}", request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Could not update the event.");
                return View(model);
            }

            // Upload a new file only if one was selected.
            if (model.EventFile != null && model.EventFile.Length > 0)
            {
                var uploadResponse = await _api.UploadFileAsync(
                    $"api/events/{Uri.EscapeDataString(id)}/file",
                    model.EventFile);

                if (!uploadResponse.IsSuccessStatusCode)
                {
                    TempData["Error"] =
                        "Event updated, but the file upload failed.";
                    return RedirectToAction("Index");
                }
            }

            TempData["Success"] = "Event updated successfully.";

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var response = await _api.DeleteAsync(
                $"api/events/{Uri.EscapeDataString(id)}");

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Could not delete the event.";
            else
                TempData["Success"] = "Event deleted successfully.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Results(string eventId)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var results = await _api.GetAsync<List<ResultViewModel>>(
                $"api/events/{Uri.EscapeDataString(eventId)}/results");

            ViewBag.EventId = eventId;

            return View(results ?? new List<ResultViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> Enrolments(string eventId)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var enrolments = await _api.GetAsync<List<EnrolmentManagementViewModel>>(
                $"api/enrolments/event/{Uri.EscapeDataString(eventId)}");

            ViewBag.EventId = eventId;

            return View(enrolments ?? new List<EnrolmentManagementViewModel>());
        }


        [HttpPost]
        public async Task<IActionResult> AddResult(string eventId, CreateResultRequest model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var response = await _api.PostAsync("api/results", model);

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Could not add the result.";
            else
                TempData["Success"] = "Result added successfully.";

            return RedirectToAction("Results", new { eventId });
        }
    }
}
