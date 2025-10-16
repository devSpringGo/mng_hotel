using System;
using System.Collections.Generic;
using ManageHotel.Models;
using Microsoft.EntityFrameworkCore;

namespace ManageHotel.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<HotelUser> HotelUsers { get; set; }

    public virtual DbSet<IcalSyncLog> IcalSyncLogs { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=THINKPAD-OF-ME;Database=WebReceptionDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.GuestId).HasName("PK__Guests__0C423C323B77379C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.HotelId).HasName("PK__Hotels__46023BBF716FDFB4");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<HotelUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__HotelUse__1788CCAC40D94FE1");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Hotel).WithMany(p => p.HotelUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HotelUser__Hotel__3C69FB99");
        });

        modelBuilder.Entity<IcalSyncLog>(entity =>
        {
            entity.HasKey(e => e.SyncId).HasName("PK__ICalSync__7E50DEA6F09DDE39");

            entity.Property(e => e.SyncTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Hotel).WithMany(p => p.IcalSyncLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ICalSyncL__Hotel__59FA5E80");

            entity.HasOne(d => d.RoomType).WithMany(p => p.IcalSyncLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ICalSyncL__RoomT__5AEE82B9");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Reports__D5BD48E50C32EB57");

            entity.Property(e => e.GeneratedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Reports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reports__HotelID__5EBF139D");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Reservat__B7EE5F04F6C14466");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Source).HasDefaultValue("Manual");
            entity.Property(e => e.Status).HasDefaultValue("Booked");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Reservations).HasConstraintName("FK__Reservati__Creat__5629CD9C");

            entity.HasOne(d => d.Guest).WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__Guest__5070F446");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__Hotel__4F7CD00D");

            entity.HasOne(d => d.Room).WithMany(p => p.Reservations).HasConstraintName("FK__Reservati__RoomI__52593CB8");

            entity.HasOne(d => d.RoomType).WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__RoomT__5165187F");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Rooms__3286391960658F71");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Available");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Rooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rooms__HotelID__46E78A0C");

            entity.HasOne(d => d.RoomType).WithMany(p => p.Rooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rooms__RoomTypeI__47DBAE45");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.RoomTypeId).HasName("PK__RoomType__BCC89611CB897D61");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.RoomCount).HasDefaultValue(0);

            entity.HasOne(d => d.Hotel).WithMany(p => p.RoomTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RoomTypes__Hotel__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
