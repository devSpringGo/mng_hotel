using ManageHotel.Data;
using ManageHotel.Models;
using ManageHotel.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManageHotel.Services.Implementions
{
    public class RoomService : IRoomService
    {
        private readonly AppDbContext _context;
        public RoomService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAllAsync(int? hotelId = null)
        
        {
            var q = _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .AsQueryable();

            if (hotelId.HasValue)
                q = q.Where(r => r.HotelId == hotelId.Value);

            return await q.OrderBy(r => r.RoomId).ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.RoomId == id);
        }

        public async Task CreateAsync(Room room, int status)
        {
            // store status as numeric string (because your model's Status is string)
            room.Status = status.ToString();
            room.CreatedAt = DateTime.Now;
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room room, int status)
        {
            var exist = await _context.Rooms.FindAsync(room.RoomId);
            if (exist == null) return;

            exist.RoomName = room.RoomName;
            exist.HotelId = room.HotelId;
            exist.RoomTypeId = room.RoomTypeId;
            exist.Notes = room.Notes;
            exist.Status = status.ToString();
            exist.LinkIcal = room.LinkIcal;
            // do not overwrite CreatedAt

            _context.Rooms.Update(exist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.Rooms.FindAsync(id);
            if (e == null) return;
            _context.Rooms.Remove(e);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Hotel>> GetHotelsAsync()
        {
            return await _context.Hotels.OrderBy(h => h.HotelName).ToListAsync();
        }

        public async Task<IEnumerable<RoomType>> GetRoomTypesByHotelAsync(int hotelId)
        {
            return await _context.RoomTypes.Where(rt => rt.HotelId == hotelId).OrderBy(rt => rt.TypeName).ToListAsync();
        }
    }
}
