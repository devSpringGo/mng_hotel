using Ical.Net;
using Ical.Net.Serialization;
using ManageHotel.Data;
using ManageHotel.Models;
using ManageHotel.Services.Interfaces;
using ManageHotel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace ManageHotel.Controllers
{
    public class ReservationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IReservationService _reservationService;

        public ReservationController(AppDbContext context, HttpClient httpClient, IReservationService reservationService)
        {
            _context = context;
            _httpClient = httpClient;
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Rooms"] = new SelectList(_context.Rooms, "RoomId", "RoomName");
            var reservations = await _reservationService.GetListReservationFromIcallink();
            return View(reservations);
        }


        // GET: Reservation/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.HotelUsers, "UserId", "UserId");
            ViewData["GuestId"] = new SelectList(_context.Guests, "GuestId", "GuestId");
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelId");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId");
            return View();
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationId,HotelId,GuestId,RoomTypeId,RoomId,CheckInDate,CheckOutDate,Notes,Status,Source,CreatedBy,CreatedAt")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.HotelUsers, "UserId", "UserId", reservation.CreatedBy);
            ViewData["GuestId"] = new SelectList(_context.Guests, "GuestId", "GuestId", reservation.GuestId);
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelId", reservation.HotelId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", reservation.RoomId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId", reservation.RoomTypeId);
            return View(reservation);
        }

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.HotelUsers, "UserId", "UserId", reservation.CreatedBy);
            ViewData["GuestId"] = new SelectList(_context.Guests, "GuestId", "GuestId", reservation.GuestId);
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelId", reservation.HotelId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", reservation.RoomId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId", reservation.RoomTypeId);
            return View(reservation);
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationId,HotelId,GuestId,RoomTypeId,RoomId,CheckInDate,CheckOutDate,Notes,Status,Source,CreatedBy,CreatedAt")] Reservation reservation)
        {
            if (id != reservation.ReservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.HotelUsers, "UserId", "UserId", reservation.CreatedBy);
            ViewData["GuestId"] = new SelectList(_context.Guests, "GuestId", "GuestId", reservation.GuestId);
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelId", reservation.HotelId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", reservation.RoomId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId", reservation.RoomTypeId);
            return View(reservation);
        }

        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.CreatedByNavigation)
                .Include(r => r.Guest)
                .Include(r => r.Hotel)
                .Include(r => r.Room)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationId == id);
        }
    }
}
