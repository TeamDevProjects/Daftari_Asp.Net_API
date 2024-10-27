using System;
using System.Collections.Generic;
using Daftari.Entities;
using Microsoft.EntityFrameworkCore;

namespace Daftari.Data;

public partial class DaftariContext : DbContext
{
    public DaftariContext()
    {
    }

    public DaftariContext(DbContextOptions<DaftariContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BusinessType> BusinessTypes { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientTransaction> ClientTransactions { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Sector> Sectors { get; set; } 

	public virtual DbSet<SectorType> SectorTypes { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTransaction> UserTransactions { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer(Settings.ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BusinessType>(entity =>
        {
            entity.HasKey(e => e.BusinessTypeId).HasName("PK__Business__1D43DEC031C4A030");

            entity.Property(e => e.BusinessTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Person).WithMany(p => p.Clients)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Clients_People");

            entity.HasOne(d => d.User).WithMany(p => p.Clients)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Clients_Users");
        });

        modelBuilder.Entity<ClientTransaction>(entity =>
        {
            entity.HasKey(e => e.ClientTransactionId).HasName("PK__ClientTr__93829C43C62F0C69");

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientTransactions)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClientTra__Clien__70DDC3D8");

            entity.HasOne(d => d.Transaction).WithMany(p => p.ClientTransactions)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClientTra__Trans__6EF57B66");

            entity.HasOne(d => d.User).WithMany(p => p.ClientTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClientTra__UserI__6FE99F9F");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sector>(entity =>
        {
            entity.HasKey(e => e.SectorId).HasName("PK__Sectors__755E57E9FF37BD72");

            entity.Property(e => e.SectorName).HasMaxLength(50);

            entity.HasOne(d => d.SectorType).WithMany(p => p.Sectors)
                .HasForeignKey(d => d.SectorTypeId)
                .HasConstraintName("FK__Sectors__SectorT__7C4F7684");
        });

        modelBuilder.Entity<SectorType>(entity =>
        {
            entity.HasKey(e => e.SectorTypeId).HasName("PK__SectorTy__1E4CA4AF813D5733");

            entity.Property(e => e.SectorTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Person).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suppliers_People");

            entity.HasOne(d => d.User).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suppliers_Users");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6B25B42628");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ImageType).HasMaxLength(10);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.TransactionTypeId)
                .HasConstraintName("FK__Transacti__Trans__60A75C0F");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.TransactionTypeId).HasName("PK__Transact__20266D0BE08A2C90");

            entity.Property(e => e.TransactionTypeId).ValueGeneratedOnAdd();
            entity.Property(e => e.TransactionTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.UserName, "UQ__Users__C9F28456B8C0A8B5").IsUnique();

            entity.HasIndex(e => e.PasswordHash, "UQ__Users__D60E46A29294EA4C").IsUnique();

            entity.Property(e => e.PasswordHash).HasMaxLength(250);
            entity.Property(e => e.StoreName).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.Person).WithMany(p => p.Users)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_People1");

            entity.HasOne(d => d.Sector).WithMany(p => p.Users)
                .HasForeignKey(d => d.SectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__SectorId__7D439ABD");
        });

        modelBuilder.Entity<UserTransaction>(entity =>
        {
            entity.HasKey(e => e.UserTransactionId).HasName("PK__UserTran__55CF47FFCAA9582A");

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Transaction).WithMany(p => p.UserTransactions)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserTrans__Trans__6A30C649");

            entity.HasOne(d => d.User).WithMany(p => p.UserTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserTrans__UserI__6B24EA82");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
