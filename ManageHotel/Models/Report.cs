using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManageHotel.Models;

public partial class Report
{
    [Key]
    [Column("ReportID")]
    public int ReportId { get; set; }

    [Column("HotelID")]
    public int HotelId { get; set; }

    [StringLength(50)]
    public string? ReportType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? GeneratedAt { get; set; }

    [StringLength(255)]
    public string? FilePath { get; set; }

    [ForeignKey("HotelId")]
    [InverseProperty("Reports")]
    public virtual Hotel Hotel { get; set; } = null!;
}
