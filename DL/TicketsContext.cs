using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DL
{
    public partial class TicketsContext : DbContext
    {
        public TicketsContext()
        {
        }

        public TicketsContext(DbContextOptions<TicketsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Resuman> Resumen { get; set; } = null!;
        public virtual DbSet<Ticket> Tickets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.; Database= Tickets; TrustServerCertificate=True; User ID=sa; Password=pass@word1;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resuman>(entity =>
            {
                entity.HasKey(e => e.IdResumen)
                    .HasName("PK__Resumen__C15B26E56FDFDD9D");

                entity.Property(e => e.IdRegistradora)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdTienda)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.IdTicket)
                    .HasName("PK__Tickets__4B93C7E72CDF34E6");

                entity.Property(e => e.FechaHoraCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaHoraTicket).HasColumnType("datetime");

                entity.Property(e => e.IdRegistradora)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdTienda)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Impuesto).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Ticket1).HasColumnName("Ticket");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
