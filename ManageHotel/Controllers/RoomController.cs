using ManageHotel.Models;
using ManageHotel.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ManageHotel.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _service;
        public RoomController(IRoomService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int? hotelId, string? status, string? keyword)
        {
            var hotels = await _service.GetHotelsAsync();
            ViewBag.Hotels = new SelectList(hotels, "HotelId", "HotelName", hotelId);

            // Build list for status filtering
            ViewBag.StatusList = new List<SelectListItem>
            {
                new SelectListItem { Text = "All Status", Value = "" },
                new SelectListItem { Text = "Available", Value = "0", Selected = (status == "0") },
                new SelectListItem { Text = "Booked", Value = "1", Selected = (status == "1") },
                new SelectListItem { Text = "Cleaning", Value = "2", Selected = (status == "2") },
                new SelectListItem { Text = "Maintenance", Value = "3", Selected = (status == "3") }
            };

            var rooms = await _service.GetAllAsync(hotelId);

            // Search by RoomName
            if (!string.IsNullOrEmpty(keyword))
                rooms = rooms.Where(r => r.RoomName.Contains(keyword, StringComparison.OrdinalIgnoreCase));

            // Filter by Status
            if (!string.IsNullOrEmpty(status) && int.TryParse(status, out int parsedStatus))
                rooms = rooms.Where(r => ParseStatusValue(r.Status) == parsedStatus);

            return View(rooms);
        }
        private int ParseStatusValue(string statusStr)
        {
            if (int.TryParse(statusStr, out var s))
                return s;

            if (string.IsNullOrWhiteSpace(statusStr))
                return 0; // Default = Available

            switch (statusStr.Trim().ToLowerInvariant())
            {
                case "available":
                case "vacant": return 0;
                case "booked":
                case "occupied": return 1;
                case "cleaning": return 2;
                case "maintenance": return 3;
                default: return 0;
            }
        }

        public async Task<IActionResult> Create()
        {
            var hotels = await _service.GetHotelsAsync();
            ViewBag.Hotels = new SelectList(hotels, "HotelId", "HotelName");
            ViewBag.RoomTypes = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.StatusList = GetStatusSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room, int status)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Hotels = new SelectList(await _service.GetHotelsAsync(), "HotelId", "HotelName", room.HotelId);
                ViewBag.RoomTypes = new SelectList(await _service.GetRoomTypesByHotelAsync(room.HotelId), "RoomTypeId", "TypeName", room.RoomTypeId);
                ViewBag.StatusList = GetStatusSelectList();
                return View(room);
            }

            await _service.CreateAsync(room, status);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var room = await _service.GetByIdAsync(id);
            if (room == null) return NotFound();

            await PopulateDropDowns(room);

            return View(room);
        }
        private async Task PopulateDropDowns(Room room, int? selectedStatus = null)
        {
            var hotels = await _service.GetHotelsAsync();
            ViewBag.Hotels = new SelectList(hotels, "HotelId", "HotelName", room.HotelId);
            ViewBag.HotelsList = hotels;

            var roomTypes = await _service.GetRoomTypesByHotelAsync(room.HotelId);
            ViewBag.RoomTypes = new SelectList(roomTypes, "RoomTypeId", "TypeName", room.RoomTypeId);
            ViewBag.RoomTypeList = roomTypes;

            int currentStatus = selectedStatus ?? (int.TryParse(room.Status, out var s) ? s : 0);
            ViewBag.StatusList = GetStatusSelectList(currentStatus);
        }

        private List<SelectListItem> GetStatusSelectList(int selected)
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "0 - Available", Value = "0", Selected = selected == 0 },
                new SelectListItem { Text = "1 - Booked", Value = "1", Selected = selected == 1 },
                new SelectListItem { Text = "2 - Cleaning", Value = "2", Selected = selected == 2 },
                new SelectListItem { Text = "3 - Maintenance", Value = "3", Selected = selected == 3 }
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Room room)
        {
            if (id != room.RoomId) return BadRequest();

            if (!ModelState.IsValid)
            {
                await PopulateDropDowns(room);
                return View(room);
            }

            int status = int.TryParse(room.Status, out var s) ? s : 0;
            await _service.UpdateAsync(room, status);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var room = await _service.GetByIdAsync(id);
            if (room == null) return NotFound();
            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Helpers
        private IEnumerable<SelectListItem> GetStatusSelectList(int? selected = null)
        {
            var list = Enum.GetValues(typeof(RoomStatus))
                           .Cast<RoomStatus>()
                           .Select(rs => new SelectListItem(((int)rs).ToString() + " - " + rs.ToString(), ((int)rs).ToString()));
            if (selected != null)
                foreach (var it in list) it.Selected = (it.Value == selected.Value.ToString());
            return list;
        }

        private int ParseStatus(string? status)
        {
            if (int.TryParse(status, out var s)) return s;
            return (int)RoomStatus.Available;
        }
    }
}
