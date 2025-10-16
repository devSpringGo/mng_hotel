using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManageHotel.Models;

[Index("HotelId", "CheckInDate", Name = "IX_Reservations_Hotel_CheckInDate")]
[Index("Status", Name = "IX_Reservations_Status")]
public partial class Reservation
{
    [Key]
    [Column("ReservationID")]
    public int ReservationId { get; set; }

    [Column("HotelID")]
    public int HotelId { get; set; }

    [Column("GuestID")]
    public int GuestId { get; set; }

    [Column("RoomTypeID")]
    public int RoomTypeId { get; set; }

    [Column("RoomID")]
    public int? RoomId { get; set; }

    public DateOnly CheckInDate { get; set; }

    public DateOnly CheckOutDate { get; set; }

    [StringLength(255)]
    public string? Notes { get; set; }

    [StringLength(30)]
    public string? Status { get; set; }

    [StringLength(50)]
    public string? Source { get; set; }

    public int? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("Reservations")]
    public virtual HotelUser? CreatedByNavigation { get; set; }

    [ForeignKey("GuestId")]
    [InverseProperty("Reservations")]
    public virtual Guest Guest { get; set; } = null!;

    [ForeignKey("HotelId")]
    [InverseProperty("Reservations")]
    public virtual Hotel Hotel { get; set; } = null!;

    [ForeignKey("RoomId")]
    [InverseProperty("Reservations")]
    public virtual Room? Room { get; set; }

    [ForeignKey("RoomTypeId")]
    [InverseProperty("Reservations")]
    public virtual RoomType RoomType { get; set; } = null!;
}
