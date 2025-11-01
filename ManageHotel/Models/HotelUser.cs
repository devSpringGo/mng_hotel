using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManageHotel.Models;

[Index("Username", Name = "UQ__HotelUse__536C85E4800FFFA6", IsUnique = true)]
public partial class HotelUser
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [Column("HotelID")]
    public int HotelId { get; set; }

    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(255)]
    public string? PasswordHash { get; set; }

    [StringLength(100)]
    public string? FullName { get; set; }

    [StringLength(50)]
    public string? Role { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    public bool? IsActive { get; set; }

    [ForeignKey("HotelId")]
    [InverseProperty("HotelUsers")]
    public virtual Hotel? Hotel { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
