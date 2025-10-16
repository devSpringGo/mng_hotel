using ManageHotel.Data;
using ManageHotel.Models;
using ManageHotel.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageHotel.Services
{
    public class HotelService : IHotelService
    {
        private readonly AppDbContext _context;
        public HotelService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync(string? keyword = null, string? isActive = null)
        {
            var q = _context.Hotels.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                q = q.Where(h => h.HotelName.Contains(keyword));
            }

            if (!string.IsNullOrWhiteSpace(isActive))
            {
                // isActive expected: "" / "1" (active) / "0" (inactive)
                if (isActive == "1")
                    q = q.Where(h => h.IsActive == true);
                else if (isActive == "0")
                    q = q.Where(h => h.IsActive == false || h.IsActive == null);
            }

            return await q.OrderBy(h => h.HotelName).ToListAsync();
        }

        public async Task<Hotel?> GetByIdAsync(int id)
        {
            return await _context.Hotels.FirstOrDefaultAsync(h => h.HotelId == id);
        }

        public async Task CreateAsync(Hotel hotel)
        {
            hotel.CreatedAt = System.DateTime.Now;
            // default IsActive true if not set
            if (hotel.IsActive == null) hotel.IsActive = true;
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Hotel hotel)
        {
            var e = await _context.Hotels.FindAsync(hotel.HotelId);
            if (e == null) return;
            e.HotelName = hotel.HotelName;
            e.Email = hotel.Email;
            e.Address = hotel.Address;
            e.IsActive = hotel.IsActive;
            // Do not update CreatedAt
            _context.Hotels.Update(e);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.Hotels.FindAsync(id);
            if (e == null) return;
            _context.Hotels.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
