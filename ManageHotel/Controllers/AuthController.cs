using ManageHotel.Models;
using ManageHotel.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace ManageHotel.Controllers
{

    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // Nếu đã login rồi thì chuyển thẳng về dashboard
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Register()
        {
            var hotels = _authService.GetHotelsAsync().Result;

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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role ?? "Receptionist")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // ✅ Đăng nhập (ghi cookie)
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

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

            if (!await _authService.RegisterAsync(model, password, HotelId))
            {
                ViewBag.Error = "Username already taken!";
                ViewBag.Hotels = new SelectList(hotels, "HotelId", "HotelName");
                return View(model);
            }
            TempData["SuccessMessage"] = "Register success!";
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Login", "Auth");
        }
    }
}
