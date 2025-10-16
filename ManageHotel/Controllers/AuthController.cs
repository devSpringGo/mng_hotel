using ManageHotel.Models;
using ManageHotel.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Register() => View();

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
        public async Task<IActionResult> Register(HotelUser model, string password)
        {
            if (!ModelState.IsValid) return View(model);

            if (!await _authService.RegisterAsync(model, password))
            {
                ViewBag.Error = "Username already taken!";
                return View(model);
            }

            return RedirectToAction("Login");
        }
    }
}
