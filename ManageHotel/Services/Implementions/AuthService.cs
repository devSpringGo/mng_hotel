using ManageHotel.Data;
using ManageHotel.Models;
using ManageHotel.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace ManageHotel.Models
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HotelUser?> LoginAsync(string username, string password)
        {
            string hashed = HashPassword(password);
            return await _context.HotelUsers.FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == hashed);
        }

        public async Task<bool> RegisterAsync(HotelUser user, string password, int hotelId)
        {
            if (await IsUsernameTakenAsync(user.Username)) return false;

            user.PasswordHash = HashPassword(password);
            user.CreatedAt = DateTime.Now;
            user.IsActive = true;
            user.HotelId = hotelId;

            _context.HotelUsers.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
            => await _context.HotelUsers.AnyAsync(u => u.Username == username);

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        public async Task<IEnumerable<Hotel>> GetHotelsAsync()
        {
            return await _context.Hotels.OrderBy(h => h.HotelName).ToListAsync();
        }

        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

    }
}
