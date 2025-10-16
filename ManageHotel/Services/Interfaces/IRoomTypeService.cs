using ManageHotel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManageHotel.Services.Interfaces
{
    public interface IRoomTypeService
    {
        Task<IEnumerable<RoomType>> GetAllAsync(int? hotelId = null, string? keyword = null);
        Task<RoomType?> GetByIdAsync(int id);
        Task CreateAsync(RoomType roomType);
        Task UpdateAsync(RoomType roomType);
        Task DeleteAsync(int id);
        Task<IEnumerable<Hotel>> GetHotelsAsync();
        Task<IEnumerable<RoomType>> GetByHotelAsync(int hotelId);
    }
}
