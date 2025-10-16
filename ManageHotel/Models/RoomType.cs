using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManageHotel.Models;

public partial class RoomType
{
    [Key]
    [Column("RoomTypeID")]
    public int RoomTypeId { get; set; }

    [Column("HotelID")]
    public int HotelId { get; set; }

    [StringLength(100)]
    public string TypeName { get; set; } = null!;

    public int? RoomCount { get; set; }

    [Column("ICalLink")]
    [StringLength(500)]
    public string? IcalLink { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("HotelId")]
    [InverseProperty("RoomTypes")]
    public virtual Hotel Hotel { get; set; } = null!;

    [InverseProperty("RoomType")]
    public virtual ICollection<IcalSyncLog> IcalSyncLogs { get; set; } = new List<IcalSyncLog>();

    [InverseProperty("RoomType")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    [InverseProperty("RoomType")]
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
