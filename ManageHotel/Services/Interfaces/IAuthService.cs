using ManageHotel.Models;
using System.Threading.Tasks;

namespace ManageHotel.Services.Interfaces
{
    public interface IAuthService
    {
        Task<HotelUser?> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(HotelUser user, string password, int hotelId);
        Task<bool> IsUsernameTakenAsync(string username);
        Task<IEnumerable<Hotel>> GetHotelsAsync();

    }
}
