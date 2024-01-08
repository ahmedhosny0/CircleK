using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CK.Model;

public partial class CkhelperdbContext : DbContext
{
    public CkhelperdbContext()
    {
    }

    public CkhelperdbContext(DbContextOptions<CkhelperdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Helpersuser> Helpersusers { get; set; }

    public virtual DbSet<Liststore> Liststores { get; set; }

    public virtual DbSet<RptStore> RptStores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.168.1.156;User ID=sa;Password=P@ssw0rd;Database=CKHelperdb;Connect Timeout=150;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Helpersuser>(entity =>
        {
            entity.HasKey(e => e.Password).HasName("PK__helpersu__6E2DBEDFBF86B99E");

            entity.ToTable("helpersusers");

            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Liststore>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("liststores");

            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.DbName)
                .HasMaxLength(50)
                .HasColumnName("dbName");
            entity.Property(e => e.DbZkname)
                .HasMaxLength(50)
                .HasColumnName("dbZKName");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstTransactionD).HasColumnType("datetime");
            entity.Property(e => e.FirstTransactionR).HasColumnType("datetime");
            entity.Property(e => e.Franchise).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LastTransactionD).HasColumnType("datetime");
            entity.Property(e => e.LastTransactionR).HasColumnType("datetime");
            entity.Property(e => e.Lat)
                .HasMaxLength(50)
                .HasColumnName("lat");
            entity.Property(e => e.Long)
                .HasMaxLength(50)
                .HasColumnName("long");
            entity.Property(e => e.PriceCategory).HasMaxLength(50);
            entity.Property(e => e.Rmsd365)
                .HasMaxLength(50)
                .HasColumnName("RMSD365");
            entity.Property(e => e.ServerName).HasMaxLength(50);
            entity.Property(e => e.StoreName).HasMaxLength(50);
            entity.Property(e => e.StoreNameD).HasMaxLength(50);
            entity.Property(e => e.StoreNameR).HasMaxLength(50);
            entity.Property(e => e.Zkip)
                .HasMaxLength(50)
                .HasColumnName("ZKIP");
        });

        modelBuilder.Entity<RptStore>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RptStores");

            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.DbName)
                .HasMaxLength(50)
                .HasColumnName("dbName");
            entity.Property(e => e.DbZkname)
                .HasMaxLength(50)
                .HasColumnName("dbZKName");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstTransactionD).HasColumnType("datetime");
            entity.Property(e => e.FirstTransactionR).HasColumnType("datetime");
            entity.Property(e => e.Franchise).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LastTransactionD).HasColumnType("datetime");
            entity.Property(e => e.LastTransactionR).HasColumnType("datetime");
            entity.Property(e => e.Lat)
                .HasMaxLength(50)
                .HasColumnName("lat");
            entity.Property(e => e.Long)
                .HasMaxLength(50)
                .HasColumnName("long");
            entity.Property(e => e.PriceCategory).HasMaxLength(50);
            entity.Property(e => e.Rmsd365)
                .HasMaxLength(50)
                .HasColumnName("RMSD365");
            entity.Property(e => e.ServerName).HasMaxLength(50);
            entity.Property(e => e.StoreNameD).HasMaxLength(50);
            entity.Property(e => e.StoreNameR).HasMaxLength(50);
            entity.Property(e => e.Zkip)
                .HasMaxLength(50)
                .HasColumnName("ZKIP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
