using ManageHotel.Models;
using ManageHotel.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManageHotel.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login() => View();
        public IActionResult Register()
        {
            var hotels = _authService.GetHotelsAsync().Result; // hoặc await nếu action là async

            ViewBag.Hotels = new SelectList(hotels, "HotelId", "HotelName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _authService.LoginAsync(username, password);
            if (user == null)
            {
                ViewBag.Error = "Invalid login!";
                return View();
            }

            // TODO: Set session / cookie
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(HotelUser model, string password, int HotelId)
        {
            var hotels = await _authService.GetHotelsAsync();

            ModelState.Remove("Hotel");
            ModelState.Remove("PasswordHash");
            
            if (!ModelState.IsValid)
            {
                // 🔹 Load lại danh sách hotel
                ViewBag.Hotels = new SelectList(hotels, "HotelId", "HotelName");

                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return View(model);
            }

            if (!await _authService.RegisterAsync(model, password,HotelId))
            {
                ViewBag.Error = "Username already taken!";
                ViewBag.Hotels = new SelectList(hotels, "HotelId", "HotelName");
                return View(model);
            }

            return RedirectToAction("Login");
        }   
    }
}
