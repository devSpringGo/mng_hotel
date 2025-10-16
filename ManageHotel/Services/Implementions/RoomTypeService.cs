using ManageHotel.Data;
using ManageHotel.Models;
using ManageHotel.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageHotel.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly AppDbContext _context;
        public RoomTypeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoomType>> GetAllAsync(int? hotelId = null, string? keyword = null)
        {
            var q = _context.RoomTypes
                .Include(rt => rt.Hotel)
                .AsQueryable();

            if (hotelId.HasValue)
                q = q.Where(x => x.HotelId == hotelId.Value);

            if (!string.IsNullOrWhiteSpace(keyword))
                q = q.Where(x => x.TypeName.Contains(keyword));

            return await q.OrderBy(x => x.TypeName).ToListAsync();
        }

        public async Task<RoomType?> GetByIdAsync(int id)
        {
            return await _context.RoomTypes
                .Include(rt => rt.Hotel)
                .FirstOrDefaultAsync(rt => rt.RoomTypeId == id);
        }

        public async Task CreateAsync(RoomType roomType)
        {
            roomType.CreatedAt = System.DateTime.Now;
            _context.RoomTypes.Add(roomType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RoomType roomType)
        {
            var e = await _context.RoomTypes.FindAsync(roomType.RoomTypeId);
            if (e == null) return;
            e.TypeName = roomType.TypeName;
            e.RoomCount = roomType.RoomCount;
            e.IcalLink = roomType.IcalLink;
            e.HotelId = roomType.HotelId;
            _context.RoomTypes.Update(e);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.RoomTypes.FindAsync(id);
            if (e == null) return;
            _context.RoomTypes.Remove(e);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Hotel>> GetHotelsAsync()
        {
            return await _context.Hotels.OrderBy(h => h.HotelName).ToListAsync();
        }

        public async Task<IEnumerable<RoomType>> GetByHotelAsync(int hotelId)
        {
            return await _context.RoomTypes.Where(rt => rt.HotelId == hotelId).OrderBy(rt => rt.TypeName).ToListAsync();
        }
    }
}
