using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManageHotel.Models;

public partial class Hotel
{
    [Key]
    [Column("HotelID")]
    public int HotelId { get; set; }

    [StringLength(150)]
    public string HotelName { get; set; } = null!;

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    public bool? IsActive { get; set; }

    [InverseProperty("Hotel")]
    public virtual ICollection<HotelUser> HotelUsers { get; set; } = new List<HotelUser>();

    [InverseProperty("Hotel")]
    public virtual ICollection<IcalSyncLog> IcalSyncLogs { get; set; } = new List<IcalSyncLog>();

    [InverseProperty("Hotel")]
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    [InverseProperty("Hotel")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    [InverseProperty("Hotel")]
    public virtual ICollection<RoomType> RoomTypes { get; set; } = new List<RoomType>();

    [InverseProperty("Hotel")]
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
