using ManageHotel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManageHotel.Services.Interfaces
{
    public interface IHotelService
    {
        Task<IEnumerable<Hotel>> GetAllAsync(string? keyword = null, string? isActive = null);
        Task<Hotel?> GetByIdAsync(int id);
        Task CreateAsync(Hotel hotel);
        Task UpdateAsync(Hotel hotel);
        Task DeleteAsync(int id);
    }
}
