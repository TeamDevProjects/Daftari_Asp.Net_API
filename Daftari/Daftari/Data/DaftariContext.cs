using System;
using System.Collections.Generic;
using Daftari.Entities;
using Daftari.Entities.Views;
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

	public virtual DbSet<ClientPaymentDate> ClientPaymentDates { get; set; }

	public virtual DbSet<ClientTransaction> ClientTransactions { get; set; }

	public virtual DbSet<PaymentDate> PaymentDates { get; set; }

	public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

	public virtual DbSet<Person> People { get; set; }

	public virtual DbSet<Sector> Sectors { get; set; }

	public virtual DbSet<SectorType> SectorTypes { get; set; }

	public virtual DbSet<Supplier> Suppliers { get; set; }

	public virtual DbSet<SupplierPaymentDate> SupplierPaymentDates { get; set; }

	public virtual DbSet<SupplierTransaction> SupplierTransactions { get; set; }

	public virtual DbSet<Transaction> Transactions { get; set; }

	public virtual DbSet<TransactionType> TransactionTypes { get; set; }

	public virtual DbSet<User> Users { get; set; }

	public virtual DbSet<UserTransaction> UserTransactions { get; set; }


	public virtual DbSet<SectorsView> SectorsViews { get; set; }


//	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//		=> optionsBuilder.UseSqlServer("Server=.;Database=Daftari2;User Id=sa;Password=sa123456;Trusted_Connection=True;TrustServerCertificate=True;");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<BusinessType>(entity =>
		{
			entity.HasKey(e => e.BusinessTypeId).HasName("PK__Business__1D43DEC0A6F4B3C7");

			entity.Property(e => e.BusinessTypeId).ValueGeneratedOnAdd();
			entity.Property(e => e.BusinessTypeName).HasMaxLength(50);
		});

		modelBuilder.Entity<Client>(entity =>
		{
			entity.HasKey(e => e.ClientId).HasName("PK__Clients__E67E1A24ABEF34EA");

			entity.Property(e => e.Notes).HasMaxLength(500);

			entity.HasOne(d => d.Person).WithMany(p => p.Clients)
				.HasForeignKey(d => d.PersonId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Clients__PersonI__5DCAEF64");

			entity.HasOne(d => d.User).WithMany(p => p.Clients)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Clients__UserId__5EBF139D");
		});

		modelBuilder.Entity<ClientPaymentDate>(entity =>
		{
			entity.HasKey(e => e.ClientPaymentDateId).HasName("PK__ClientPa__2267EA1929E00C1D");

			entity.HasOne(d => d.Client).WithMany(p => p.ClientPaymentDates)
				.HasForeignKey(d => d.ClientId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ClientPay__Clien__02084FDA");

			entity.HasOne(d => d.PaymentDate).WithMany(p => p.ClientPaymentDates)
				.HasForeignKey(d => d.PaymentDateId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ClientPay__Payme__02FC7413");

			entity.HasOne(d => d.User).WithMany(p => p.ClientPaymentDates)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ClientPay__UserI__01142BA1");
		});

		modelBuilder.Entity<ClientTransaction>(entity =>
		{
			entity.HasKey(e => e.ClientTransactionId).HasName("PK__ClientTr__93829C43B5A2CE91");

			entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");

			entity.HasOne(d => d.Client).WithMany(p => p.ClientTransactions)
				.HasForeignKey(d => d.ClientId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ClientTra__Clien__6C190EBB");

			entity.HasOne(d => d.Transaction).WithMany(p => p.ClientTransactions)
				.HasForeignKey(d => d.TransactionId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ClientTra__Trans__6A30C649");

			entity.HasOne(d => d.User).WithMany(p => p.ClientTransactions)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ClientTra__UserI__6B24EA82");
		});

		modelBuilder.Entity<PaymentDate>(entity =>
		{
			entity.HasKey(e => e.PaymentDateId).HasName("PK__PaymentD__E842F4545C4B690E");

			entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
			entity.Property(e => e.Notes).HasMaxLength(500);
			entity.Property(e => e.PaymentDate1)
				.HasDefaultValueSql("(dateadd(day,(30),getdate()))")
				.HasColumnName("PaymentDate");
			entity.Property(e => e.PaymentMethod)
				.HasMaxLength(50)
				.HasDefaultValueSql("((1))");
		});

		modelBuilder.Entity<PaymentMethod>(entity =>
		{
			entity.HasKey(e => e.PaymentMethodId).HasName("PK__PaymentM__DC31C1D332740429");

			entity.Property(e => e.PaymentMethodId).ValueGeneratedOnAdd();
			entity.Property(e => e.PaymentMethodName).HasMaxLength(50);
		});

		modelBuilder.Entity<Person>(entity =>
		{
			entity.HasKey(e => e.PersonId).HasName("PK__People__AA2FFBE5F8681F14");

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
			entity.HasKey(e => e.SectorId).HasName("PK__Sectors__755E57E9494001DC");

			entity.Property(e => e.SectorId).ValueGeneratedOnAdd();
			entity.Property(e => e.SectorName).HasMaxLength(50);

			entity.HasOne(d => d.SectorType).WithMany(p => p.Sectors)
				.HasForeignKey(d => d.SectorTypeId)
				.HasConstraintName("FK__Sectors__SectorT__412EB0B6");
		});

		modelBuilder.Entity<SectorType>(entity =>
		{
			entity.HasKey(e => e.SectorTypeId).HasName("PK__SectorTy__1E4CA4AFBB7D8A9B");

			entity.Property(e => e.SectorTypeId).ValueGeneratedOnAdd();
			entity.Property(e => e.SectorTypeName).HasMaxLength(50);
		});

		modelBuilder.Entity<Supplier>(entity =>
		{
			entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B4CE8D3987");

			entity.Property(e => e.Notes).HasMaxLength(500);

			entity.HasOne(d => d.Person).WithMany(p => p.Suppliers)
				.HasForeignKey(d => d.PersonId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Suppliers__Perso__619B8048");

			entity.HasOne(d => d.User).WithMany(p => p.Suppliers)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Suppliers__UserI__628FA481");
		});

		modelBuilder.Entity<SupplierPaymentDate>(entity =>
		{
			entity.HasKey(e => e.SupplierPaymentDateId).HasName("PK__Supplier__59A46704F8B1D8BC");

			entity.HasOne(d => d.PaymentDate).WithMany(p => p.SupplierPaymentDates)
				.HasForeignKey(d => d.PaymentDateId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SupplierP__Payme__7E37BEF6");

			entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierPaymentDates)
				.HasForeignKey(d => d.SupplierId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SupplierP__Suppl__7D439ABD");

			entity.HasOne(d => d.User).WithMany(p => p.SupplierPaymentDates)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SupplierP__UserI__7C4F7684");
		});

		modelBuilder.Entity<SupplierTransaction>(entity =>
		{
			entity.HasKey(e => e.SupplierTransactionId).HasName("PK__Supplier__E905F1797BD50407");

			entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");

			entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierTransactions)
				.HasForeignKey(d => d.SupplierId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SupplierT__Suppl__71D1E811");

			entity.HasOne(d => d.Transaction).WithMany(p => p.SupplierTransactions)
				.HasForeignKey(d => d.TransactionId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SupplierT__Trans__6FE99F9F");

			entity.HasOne(d => d.User).WithMany(p => p.SupplierTransactions)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SupplierT__UserI__70DDC3D8");
		});

		modelBuilder.Entity<Transaction>(entity =>
		{
			entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6BEF1CB2CE");

			entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
			entity.Property(e => e.ImageType).HasMaxLength(10);
			entity.Property(e => e.Notes).HasMaxLength(500);
			entity.Property(e => e.TransactionDate)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");

			entity.HasOne(d => d.TransactionType).WithMany(p => p.Transactions)
				.HasForeignKey(d => d.TransactionTypeId)
				.HasConstraintName("FK__Transacti__Trans__45F365D3");
		});

		modelBuilder.Entity<TransactionType>(entity =>
		{
			entity.HasKey(e => e.TransactionTypeId).HasName("PK__Transact__20266D0B042D06F6");

			entity.Property(e => e.TransactionTypeId).ValueGeneratedOnAdd();
			entity.Property(e => e.TransactionTypeName).HasMaxLength(50);
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C386FE574");

			entity.Property(e => e.PasswordHash).HasMaxLength(250);
			entity.Property(e => e.RefreshToken)
				.HasMaxLength(255)
				.HasDefaultValue("");
			entity.Property(e => e.RefreshTokenExpiryTime)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");
			entity.Property(e => e.StoreName).HasMaxLength(50);
			entity.Property(e => e.UserName).HasMaxLength(50);
			entity.Property(e => e.UserType)
				.HasMaxLength(50)
				.HasDefaultValue("user");

			entity.HasOne(d => d.BusinessType).WithMany(p => p.Users)
				.HasForeignKey(d => d.BusinessTypeId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Users__BusinessT__59FA5E80");

			entity.HasOne(d => d.Person).WithMany(p => p.Users)
				.HasForeignKey(d => d.PersonId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Users__PersonId__5629CD9C");

			entity.HasOne(d => d.Sector).WithMany(p => p.Users)
				.HasForeignKey(d => d.SectorId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Users__SectorId__571DF1D5");
		});

		modelBuilder.Entity<UserTransaction>(entity =>
		{
			entity.HasKey(e => e.UserTransactionId).HasName("PK__UserTran__55CF47FF90BACBAB");

			entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");

			entity.HasOne(d => d.Transaction).WithMany(p => p.UserTransactions)
				.HasForeignKey(d => d.TransactionId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__UserTrans__Trans__656C112C");

			entity.HasOne(d => d.User).WithMany(p => p.UserTransactions)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__UserTrans__UserI__66603565");
		});


		modelBuilder.Entity<SectorsView>(entity =>
		{
			entity.HasNoKey();  // Since it's a view
			entity.ToView("SectorsView");  // Map to the actual database view
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
