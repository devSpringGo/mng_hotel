using ManageHotel.Models;
using ManageHotel.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace ManageHotel.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelService _service;
        public HotelController(IHotelService service)
        {
            _service = service;
        }

        // GET: /Hotel
        public async Task<IActionResult> Index(string? keyword, string? isActive)
        {
            ViewBag.Keyword = keyword ?? "";
            // isActive: "" (all), "1" active, "0" inactive
            ViewBag.IsActive = isActive ?? "";
            ViewBag.StatusList = new[]
            {
                new SelectListItem{ Text = "All", Value = "" },
                new SelectListItem{ Text = "Active", Value = "1", Selected = (isActive == "1") },
                new SelectListItem{ Text = "Inactive", Value = "0", Selected = (isActive == "0") }
            };

            var list = await _service.GetAllAsync(keyword, isActive);
            return View(list);
        }

        public IActionResult Create()
        {
            return View(new Hotel { IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hotel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _service.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hotel model)
        {
            if (id != model.HotelId) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            await _service.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
