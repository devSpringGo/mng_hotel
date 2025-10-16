using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManageHotel.Models;

[Index("FullName", Name = "IX_Guests_FullName")]
public partial class Guest
{
    [Key]
    [Column("GuestID")]
    public int GuestId { get; set; }

    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Guest")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
