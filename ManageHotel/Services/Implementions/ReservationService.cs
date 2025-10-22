using Ical.Net;
using ManageHotel.Data;
using ManageHotel.Services.Interfaces;
using ManageHotel.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace ManageHotel.Services.Implementions
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _context;
        public ReservationService(AppDbContext context)
        {
            _context = context;
        }

        //get reservation from ical link nobed
        public async Task<List<BookingEvent>> GetListReservationFromIcallink()
        {
            var rooms = await _context.Rooms.ToListAsync();
            var result = new List<BookingEvent>();

            using (var client = new HttpClient())
            {
                foreach (var room in rooms)
                {
                    if (string.IsNullOrWhiteSpace(room.LinkIcal))
                        continue;

                    try
                    {
                        string icsData = await client.GetStringAsync(room.LinkIcal);
                        var calendars = CalendarCollection.Load(icsData);
                        var calendar = calendars.FirstOrDefault();

                        if (calendar != null)
                        {
                            foreach (var e in calendar.Events)
                            {
                                string description = e.Description?.Replace("\\n", "\n") ?? "";
                                string propertyName = null;

                                var lines = description.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                                foreach (var line in lines)
                                {
                                    if (line.Trim().StartsWith("PROPERTY:", StringComparison.OrdinalIgnoreCase))
                                    {
                                        propertyName = line.Replace("PROPERTY:", "").Trim();
                                        break;
                                    }
                                }

                                result.Add(new BookingEvent
                                {
                                    RoomName = propertyName ?? "",
                                    Summary = e.Summary,
                                    Description = description,
                                    Start = e.Start.AsSystemLocal,
                                    End = e.End.AsSystemLocal,
                                    Uid = e.Uid,
                                    Created = e.Created?.AsSystemLocal,
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Cant get link ical for '{room.RoomName}': {ex.Message}");
                    }
                }
            }

            return result;
        }

    }
}
