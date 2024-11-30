
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

	public virtual DbSet<ClientTotalAmount> ClientTotalAmounts { get; set; }

	public virtual DbSet<ClientTransaction> ClientTransactions { get; set; }

	public virtual DbSet<PaymentDate> PaymentDates { get; set; }

	public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

	public virtual DbSet<Person> People { get; set; }

	public virtual DbSet<Sector> Sectors { get; set; }

	public virtual DbSet<SectorType> SectorTypes { get; set; }

	public virtual DbSet<Supplier> Suppliers { get; set; }

	public virtual DbSet<SupplierPaymentDate> SupplierPaymentDates { get; set; }

	public virtual DbSet<SupplierTotalAmount> SupplierTotalAmounts { get; set; }

	public virtual DbSet<SupplierTransaction> SupplierTransactions { get; set; }

	public virtual DbSet<Transaction> Transactions { get; set; }

	public virtual DbSet<TransactionType> TransactionTypes { get; set; }

	public virtual DbSet<User> Users { get; set; }

	public virtual DbSet<UserTotalAmount> UserTotalAmounts { get; set; }

	public virtual DbSet<UserTransaction> UserTransactions { get; set; }

	// DbSet for views
	public virtual DbSet<SectorsView> SectorsViews { get; set; }
	public virtual DbSet<ClientsPaymentDateView> ClientsPaymentDateViews { get; set; }

	public virtual DbSet<ClientsTransactionsView> ClientsTransactionsViews { get; set; }

	public virtual DbSet<ClientsView> ClientsViews { get; set; }

	public virtual DbSet<SuppliersPaymentDateView> SuppliersPaymentDateViews { get; set; }

	public virtual DbSet<SuppliersTransactionsView> SuppliersTransactionsViews { get; set; }

	public virtual DbSet<SuppliersView> SuppliersViews { get; set; }

	public virtual DbSet<UserTransactionsView> UserTransactionsViews { get; set; }

	public virtual DbSet<UsersView> UsersViews { get; set; }


	//	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
	//		=> optionsBuilder.UseSqlServer("Server=.;Database=Daftari;User Id=sa;Password=sa123456;Trusted_Connection=True;TrustServerCertificate=True;");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<BusinessType>(entity =>
		{
			entity.HasKey(e => e.BusinessTypeId).HasName("PK__Business__1D43DEC0E2317468");

			entity.Property(e => e.BusinessTypeId).ValueGeneratedOnAdd();
			entity.Property(e => e.BusinessTypeName).HasMaxLength(50);
		});

		modelBuilder.Entity<Client>(entity =>
		{
			entity.HasKey(e => e.ClientId).HasName("PK__Clients__E67E1A248096CCF6");

			entity.Property(e => e.Notes).HasMaxLength(500);

			entity.HasOne(d => d.Person).WithMany(p => p.Clients)
				.HasForeignKey(d => d.PersonId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Clients__PersonI__4E88ABD4");

			entity.HasOne(d => d.User).WithMany(p => p.Clients)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Clients__UserId__4F7CD00D");
		});

		modelBuilder.Entity<ClientPaymentDate>(entity =>
		{
			entity.HasKey(e => e.ClientPaymentDateId).HasName("PK__ClientPa__2267EA19989C382E");

			entity.HasOne(d => d.Client).WithMany(p => p.ClientPaymentDates)
				.HasForeignKey(d => d.ClientId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ClientPay__Clien__2A164134");

			entity.HasOne(d => d.PaymentDate).WithMany(p => p.ClientPaymentDates)
				.HasForeignKey(d => d.PaymentDateId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ClientPay__Payme__2B0A656D");

			entity.HasOne(d => d.User).WithMany(p => p.ClientPaymentDates)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ClientPay__UserI__29221CFB");
		});

		modelBuilder.Entity<ClientTotalAmount>(entity =>
		{
			entity.HasKey(e => e.ClientTotalAmountId).HasName("PK__ClientTo__8CD4709020114CCA");

			entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
			entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

			entity.HasOne(d => d.Client).WithMany(p => p.ClientTotalAmounts)
				.HasForeignKey(d => d.ClientId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ClientTot__Clien__2DE6D218");

			entity.HasOne(d => d.User).WithMany(p => p.ClientTotalAmounts)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ClientTot__UserI__2EDAF651");
		});

		modelBuilder.Entity<ClientTransaction>(entity =>
		{
			entity.HasKey(e => e.ClientTransactionId).HasName("PK__ClientTr__93829C43DD1852D9");

			entity.HasOne(d => d.Client).WithMany(p => p.ClientTransactions)
				.HasForeignKey(d => d.ClientId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ClientTra__Clien__4A8310C6");

			entity.HasOne(d => d.Transaction).WithMany(p => p.ClientTransactions)
				.HasForeignKey(d => d.TransactionId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ClientTra__Trans__489AC854");

			entity.HasOne(d => d.User).WithMany(p => p.ClientTransactions)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ClientTra__UserI__498EEC8D");
		});

		modelBuilder.Entity<PaymentDate>(entity =>
		{
			entity.HasKey(e => e.PaymentDateId).HasName("PK__PaymentD__E842F454B92E4780");

			entity.Property(e => e.DateOfPayment).HasDefaultValueSql("(dateadd(day,(30),getdate()))");
			entity.Property(e => e.Notes).HasMaxLength(500);
			entity.Property(e => e.PaymentMethodId).HasDefaultValue((byte)1);

			entity.HasOne(d => d.PaymentMethod).WithMany(p => p.PaymentDates)
				.HasForeignKey(d => d.PaymentMethodId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__PaymentDa__Payme__2180FB33");
		});

		modelBuilder.Entity<PaymentMethod>(entity =>
		{
			entity.HasKey(e => e.PaymentMethodId).HasName("PK__PaymentM__DC31C1D3ADAB535C");

			entity.Property(e => e.PaymentMethodId).ValueGeneratedOnAdd();
			entity.Property(e => e.PaymentMethodName).HasMaxLength(50);
		});

		modelBuilder.Entity<Person>(entity =>
		{
			entity.HasKey(e => e.PersonId).HasName("PK__People__AA2FFBE5C5D67244");

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
			entity.HasKey(e => e.SectorId).HasName("PK__Sectors__755E57E99F2B0F1F");

			entity.Property(e => e.SectorId).ValueGeneratedOnAdd();
			entity.Property(e => e.SectorName).HasMaxLength(50);

			entity.HasOne(d => d.SectorType).WithMany(p => p.Sectors)
				.HasForeignKey(d => d.SectorTypeId)
				.HasConstraintName("FK__Sectors__SectorT__4316F928");
		});

		modelBuilder.Entity<SectorType>(entity =>
		{
			entity.HasKey(e => e.SectorTypeId).HasName("PK__SectorTy__1E4CA4AFBDA7AD8B");

			entity.Property(e => e.SectorTypeId).ValueGeneratedOnAdd();
			entity.Property(e => e.SectorTypeName).HasMaxLength(50);
		});

		

		modelBuilder.Entity<Supplier>(entity =>
		{
			entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B49241C1FB");

			entity.Property(e => e.Notes).HasMaxLength(500);

			entity.HasOne(d => d.Person).WithMany(p => p.Suppliers)
				.HasForeignKey(d => d.PersonId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Suppliers__Perso__52593CB8");

			entity.HasOne(d => d.User).WithMany(p => p.Suppliers)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Suppliers__UserI__534D60F1");
		});

		modelBuilder.Entity<SupplierPaymentDate>(entity =>
		{
			entity.HasKey(e => e.SupplierPaymentDateId).HasName("PK__Supplier__59A46704603B1F27");

			entity.HasOne(d => d.PaymentDate).WithMany(p => p.SupplierPaymentDates)
				.HasForeignKey(d => d.PaymentDateId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SupplierP__Payme__2645B050");

			entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierPaymentDates)
				.HasForeignKey(d => d.SupplierId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SupplierP__Suppl__25518C17");

			entity.HasOne(d => d.User).WithMany(p => p.SupplierPaymentDates)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SupplierP__UserI__245D67DE");
		});

		modelBuilder.Entity<SupplierTotalAmount>(entity =>
		{
			entity.HasKey(e => e.SupplierTotalAmountId).HasName("PK__Supplier__266E965A4B525E7B");

			entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
			entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

			entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierTotalAmounts)
				.HasForeignKey(d => d.SupplierId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SupplierT__Suppl__339FAB6E");

			entity.HasOne(d => d.User).WithMany(p => p.SupplierTotalAmounts)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SupplierT__UserI__3493CFA7");
		});

		modelBuilder.Entity<SupplierTransaction>(entity =>
		{
			entity.HasKey(e => e.SupplierTransactionId).HasName("PK__Supplier__E905F179BD2D22F4");

			entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierTransactions)
				.HasForeignKey(d => d.SupplierId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SupplierT__Suppl__503BEA1C");

			entity.HasOne(d => d.Transaction).WithMany(p => p.SupplierTransactions)
				.HasForeignKey(d => d.TransactionId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SupplierT__Trans__4E53A1AA");

			entity.HasOne(d => d.User).WithMany(p => p.SupplierTransactions)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SupplierT__UserI__4F47C5E3");
		});

		modelBuilder.Entity<Transaction>(entity =>
		{
			entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6B6562D3E5");

			entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
			entity.Property(e => e.ImageType).HasMaxLength(10);
			entity.Property(e => e.Notes).HasMaxLength(500);
			entity.Property(e => e.TransactionDate)
				.HasDefaultValueSql("(getdate())")
				.HasColumnType("datetime");

			entity.HasOne(d => d.TransactionType).WithMany(p => p.Transactions)
				.HasForeignKey(d => d.TransactionTypeId)
				.HasConstraintName("FK__Transacti__Trans__3F115E1A");
		});

		modelBuilder.Entity<TransactionType>(entity =>
		{
			entity.HasKey(e => e.TransactionTypeId).HasName("PK__Transact__20266D0B72E9F8D5");

			entity.Property(e => e.TransactionTypeId).ValueGeneratedOnAdd();
			entity.Property(e => e.TransactionTypeName).HasMaxLength(50);
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CEE2123DA");

			entity.ToTable(tb => tb.HasTrigger("trg_AfterDelete_User"));

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
				.HasConstraintName("FK__Users__BusinessT__4AB81AF0");

			entity.HasOne(d => d.Person).WithMany(p => p.Users)
				.HasForeignKey(d => d.PersonId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Users__PersonId__46E78A0C");

			entity.HasOne(d => d.Sector).WithMany(p => p.Users)
				.HasForeignKey(d => d.SectorId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Users__SectorId__47DBAE45");
		});

		modelBuilder.Entity<UserTotalAmount>(entity =>
		{
			entity.HasKey(e => e.UserTotalAmountId).HasName("PK__UserTota__72480AB606610746");

			entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
			entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

			entity.HasOne(d => d.User).WithMany(p => p.UserTotalAmounts)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__UserTotal__UserI__395884C4");
		});

		modelBuilder.Entity<UserTransaction>(entity =>
		{
			entity.HasKey(e => e.UserTransactionId).HasName("PK__UserTran__55CF47FF963BF32D");

			entity.HasOne(d => d.Transaction).WithMany(p => p.UserTransactions)
				.HasForeignKey(d => d.TransactionId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__UserTrans__Trans__43D61337");

			entity.HasOne(d => d.User).WithMany(p => p.UserTransactions)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__UserTrans__UserI__44CA3770");
		});

		modelBuilder.Entity<SectorsView>(entity =>
		{
			entity
				.HasNoKey()
				.ToView("SectorsView");

			entity.Property(e => e.SectorName).HasMaxLength(50);
			entity.Property(e => e.SectorTypeName).HasMaxLength(50);
		});

		modelBuilder.Entity<ClientsPaymentDateView>(entity =>
		{
			entity
				.HasNoKey()
				.ToView("ClientsPaymentDateView");

			entity.Property(e => e.Name).HasMaxLength(50);
			entity.Property(e => e.Notes).HasMaxLength(500);
			entity.Property(e => e.PaymentMethodName).HasMaxLength(50);
			entity.Property(e => e.Phone)
				.HasMaxLength(15)
				.IsUnicode(false);
		});

		modelBuilder.Entity<ClientsTransactionsView>(entity =>
		{
			entity
				.HasNoKey()
				.ToView("ClientsTransactionsView");

			entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
			entity.Property(e => e.ImageType).HasMaxLength(10);
			entity.Property(e => e.Notes).HasMaxLength(500);
			entity.Property(e => e.TransactionDate).HasColumnType("datetime");
			entity.Property(e => e.TransactionTypeName).HasMaxLength(50);
		});

		modelBuilder.Entity<ClientsView>(entity =>
		{
			entity
				.HasNoKey()
				.ToView("ClientsView");

			entity.Property(e => e.Address).HasMaxLength(100);
			entity.Property(e => e.City).HasMaxLength(50);
			entity.Property(e => e.Country).HasMaxLength(50);
			entity.Property(e => e.Name).HasMaxLength(50);
			entity.Property(e => e.PaymentMethodName).HasMaxLength(50);
			entity.Property(e => e.Phone)
				.HasMaxLength(15)
				.IsUnicode(false);
			entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
		});

		modelBuilder.Entity<SuppliersPaymentDateView>(entity =>
		{
			entity
				.HasNoKey()
				.ToView("SuppliersPaymentDateView");

			entity.Property(e => e.Name).HasMaxLength(50);
			entity.Property(e => e.Notes).HasMaxLength(500);
			entity.Property(e => e.PaymentMethodName).HasMaxLength(50);
			entity.Property(e => e.Phone)
				.HasMaxLength(15)
				.IsUnicode(false);
		});

		modelBuilder.Entity<SuppliersTransactionsView>(entity =>
		{
			entity
				.HasNoKey()
				.ToView("SuppliersTransactionsView");

			entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
			entity.Property(e => e.ImageType).HasMaxLength(10);
			entity.Property(e => e.Notes).HasMaxLength(500);
			entity.Property(e => e.TransactionDate).HasColumnType("datetime");
			entity.Property(e => e.TransactionTypeName).HasMaxLength(50);
		});

		modelBuilder.Entity<SuppliersView>(entity =>
		{
			entity
				.HasNoKey()
				.ToView("SuppliersView");

			entity.Property(e => e.Address).HasMaxLength(100);
			entity.Property(e => e.City).HasMaxLength(50);
			entity.Property(e => e.Country).HasMaxLength(50);
			entity.Property(e => e.Name).HasMaxLength(50);
			entity.Property(e => e.Notes).HasMaxLength(500);
			entity.Property(e => e.PaymentMethodName).HasMaxLength(50);
			entity.Property(e => e.Phone)
				.HasMaxLength(15)
				.IsUnicode(false);
			entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
		});

		modelBuilder.Entity<UserTransactionsView>(entity =>
		{
			entity
				.HasNoKey()
				.ToView("UserTransactionsView");

			entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
			entity.Property(e => e.ImageType).HasMaxLength(10);
			entity.Property(e => e.Notes).HasMaxLength(500);
			entity.Property(e => e.TransactionDate).HasColumnType("datetime");
			entity.Property(e => e.TransactionTypeName).HasMaxLength(50);
		});

		modelBuilder.Entity<UsersView>(entity =>
		{
			entity
				.HasNoKey()
				.ToView("UsersView");

			entity.Property(e => e.Address).HasMaxLength(100);
			entity.Property(e => e.BusinessTypeName).HasMaxLength(50);
			entity.Property(e => e.City).HasMaxLength(50);
			entity.Property(e => e.Country).HasMaxLength(50);
			entity.Property(e => e.Name).HasMaxLength(50);
			entity.Property(e => e.Phone)
				.HasMaxLength(15)
				.IsUnicode(false);
			entity.Property(e => e.SectorName).HasMaxLength(50);
			entity.Property(e => e.SectorTypeName).HasMaxLength(50);
			entity.Property(e => e.StoreName).HasMaxLength(50);
			entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
			entity.Property(e => e.UserName).HasMaxLength(50);
			entity.Property(e => e.UserType).HasMaxLength(50);
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
