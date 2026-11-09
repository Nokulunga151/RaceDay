using Microsoft.AspNetCore.Mvc;
using RaceDay.Contracts;
using RaceDay.Mvc.Services;
using RaceDay.Mvc.ViewModels;

namespace RaceDay.Mvc.Controllers
{
    public class EventsController : Controller
    {
        private readonly RaceDayApiClient _api;

        public EventsController(RaceDayApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _api.GetAsync<List<EventViewModel>>(
                "api/events");

            return View(events ?? new List<EventViewModel>());
        }

        public async Task<IActionResult> Details(string id)
        {
            var raceEvent = await _api.GetAsync<EventViewModel>(
                $"api/events/{Uri.EscapeDataString(id)}");

            if (raceEvent == null)
                return NotFound();

            var categories = await _api.GetAsync<List<CategoryDto>>(
                $"api/events/{Uri.EscapeDataString(id)}/categories");

            var model = new EventDetailsViewModel
            {
                Event = raceEvent,
                Categories = categories ?? new List<CategoryDto>()
            };

            return View(model);
        }
    }
}
