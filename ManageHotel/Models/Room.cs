using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManageHotel.Models;

public partial class Room
{
    [Key]
    [Column("RoomID")]
    public int RoomId { get; set; }

    [Column("HotelID")]
    public int HotelId { get; set; }

    [Column("RoomTypeID")]
    public int RoomTypeId { get; set; }

    [Column("RoomName")]
    [StringLength(50)]
    public string RoomName { get; set; } = null!;

    [Column("Status")]
    [StringLength(20)]
    public string? Status { get; set; }

    [Column("Notes")]
    [StringLength(255)]
    public string? Notes { get; set; }

    [Column("CreatedAt", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("LinkIcal")]
    [StringLength(200)]
    public string? LinkIcal { get; set; }

    [ForeignKey("HotelId")]
    [InverseProperty("Rooms")]
    public virtual Hotel Hotel { get; set; } = null!;

    [InverseProperty("Room")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    [ForeignKey("RoomTypeId")]
    [InverseProperty("Rooms")]
    public virtual RoomType RoomType { get; set; } = null!;
}
