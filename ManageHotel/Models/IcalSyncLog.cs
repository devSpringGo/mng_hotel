using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManageHotel.Models;

[Table("ICalSyncLog")]
public partial class IcalSyncLog
{
    [Key]
    [Column("SyncID")]
    public int SyncId { get; set; }

    [Column("HotelID")]
    public int HotelId { get; set; }

    [Column("RoomTypeID")]
    public int RoomTypeId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SyncTime { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [StringLength(255)]
    public string? Message { get; set; }

    [ForeignKey("HotelId")]
    [InverseProperty("IcalSyncLogs")]
    public virtual Hotel Hotel { get; set; } = null!;

    [ForeignKey("RoomTypeId")]
    [InverseProperty("IcalSyncLogs")]
    public virtual RoomType RoomType { get; set; } = null!;
}
