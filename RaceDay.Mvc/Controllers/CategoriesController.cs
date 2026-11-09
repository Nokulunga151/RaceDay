using Microsoft.AspNetCore.Mvc;
using RaceDay.Contracts;
using RaceDay.Mvc.Services;

namespace RaceDay.Mvc.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly RaceDayApiClient _api;

        public CategoriesController(RaceDayApiClient api)
        {
            _api = api;
        }

        private bool IsOrganiser()
        {
            return HttpContext.Session.GetString("Role") == "Organiser";
        }

        // Display categories belonging to an event.
        public async Task<IActionResult> Index(string eventId)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var categories = await _api.GetAsync<List<CategoryDto>>(
                $"api/events/{eventId}/categories");

            ViewBag.EventId = eventId;

            return View(categories ?? new List<CategoryDto>());
        }



        // Display the create category form.
        [HttpGet]
        public IActionResult Create(string eventId)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            ViewBag.EventId = eventId;

            return View();
        }

        // Submit a new category.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string eventId,
            CreateCategoryRequest model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                ViewBag.EventId = eventId;
                return View(model);
            }

            var response = await _api.PostAsync(
                $"api/events/{eventId}/categories", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Category created successfully.";
                return RedirectToAction("Index", new { eventId });
            }

            var error = await response.Content.ReadAsStringAsync();

            ModelState.AddModelError("",
                $"Unable to create category. API says: {response.StatusCode} - {error}");

            ViewBag.EventId = eventId;
            return View(model);
        }

        // Display the edit category form.
        [HttpGet]
        public async Task<IActionResult> Edit(string eventId, string id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var categories = await _api.GetAsync<List<CategoryDto>>(
                $"api/events/{eventId}/categories");

            var category = categories?.FirstOrDefault(c => c.CategoryID == id);

            if (category == null)
                return NotFound();

            var model = new UpdateCategoryRequest
            {
                CategoryName = category.CategoryName,
                Distance = category.Distance,
                EntryFee = category.EntryFee
            };

            ViewBag.EventId = eventId;
            ViewBag.CategoryId = id;

            return View(model);
        }



        // Submit category changes.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            string eventId,
            string id,
            UpdateCategoryRequest model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                ViewBag.EventId = eventId;
                return View(model);
            }

            var response = await _api.PutAsync(
                $"api/categories/{id}", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Category updated successfully.";
                return RedirectToAction("Index", new { eventId });
            }

            ModelState.AddModelError("", "Unable to update category.");
            ViewBag.EventId = eventId;

            return View(model);
        }

        // Delete a category.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string eventId, string id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var response = await _api.DeleteAsync(
                $"api/categories/{id}");

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Category deleted successfully.";
            else
                TempData["Error"] = "Unable to delete category.";

            return RedirectToAction("Index", new { eventId });
        }
    }
}
