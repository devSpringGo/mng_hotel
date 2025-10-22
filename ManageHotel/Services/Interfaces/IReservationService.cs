using ManageHotel.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManageHotel.Services.Interfaces
{
    public interface IReservationService
    {
        Task<List<BookingEvent>> GetListReservationFromIcallink();
    }
}
