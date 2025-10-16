using ManageHotel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ManageHotel.Services.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllAsync(int? hotelId = null);
        Task<Room?> GetByIdAsync(int id);
        Task CreateAsync(Room room, int status);
        Task UpdateAsync(Room room, int status);
        Task DeleteAsync(int id);
        Task<IEnumerable<Hotel>> GetHotelsAsync();
        Task<IEnumerable<RoomType>> GetRoomTypesByHotelAsync(int hotelId);
    }
}
