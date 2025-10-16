using ManageHotel.Models;
using ManageHotel.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace ManageHotel.Controllers
{
    public class RoomTypeController : Controller
    {
        private readonly IRoomTypeService _service;
        public RoomTypeController(IRoomTypeService service)
        {
            _service = service;
        }

        // GET: /RoomType
        public async Task<IActionResult> Index(int? hotelId, string? keyword)
        {
            var hotels = await _service.GetHotelsAsync();
            ViewBag.Hotels = new SelectList(hotels, "HotelId", "HotelName", hotelId);
            ViewBag.CurrentHotel = hotelId?.ToString() ?? "";
            ViewBag.Keyword = keyword ?? "";

            var list = await _service.GetAllAsync(hotelId, keyword);
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Hotels = new SelectList(await _service.GetHotelsAsync(), "HotelId", "HotelName");
            return View(new RoomType());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomType model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Hotels = new SelectList(await _service.GetHotelsAsync(), "HotelId", "HotelName", model.HotelId);
                return View(model);
            }

            await _service.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            ViewBag.Hotels = new SelectList(await _service.GetHotelsAsync(), "HotelId", "HotelName", item.HotelId);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomType model)
        {
            if (id != model.RoomTypeId) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Hotels = new SelectList(await _service.GetHotelsAsync(), "HotelId", "HotelName", model.HotelId);
                return View(model);
            }

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

        // API: get roomtypes by hotel (JSON) - useful for AJAX dropdowns
        [HttpGet("/api/roomtypes/byhotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(int hotelId)
        {
            var list = await _service.GetByHotelAsync(hotelId);
            // return simple DTO
            var dto = list.Select(rt => new { roomTypeId = rt.RoomTypeId, typeName = rt.TypeName });
            return Json(dto);
        }
    }
}
