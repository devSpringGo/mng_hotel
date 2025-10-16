using ManageHotel.Models;
using System.Threading.Tasks;

namespace ManageHotel.Services.Interfaces
{
    public interface IAuthService
    {
        Task<HotelUser?> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(HotelUser user, string password);
        Task<bool> IsUsernameTakenAsync(string username);
    }
}
