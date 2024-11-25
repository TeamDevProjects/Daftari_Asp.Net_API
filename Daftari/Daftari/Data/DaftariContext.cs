
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

    public virtual DbSet<ClientPaymentDate> ClientPaymentDates { get; set; }

    public virtual DbSet<ClientTotalAmount> ClientTotalAmounts { get; set; }

    public virtual DbSet<ClientTransaction> ClientTransactions { get; set; }

    public virtual DbSet<PaymentDate> PaymentDates { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Sector> Sectors { get; set; }

    public virtual DbSet<SectorType> SectorTypes { get; set; }

    public virtual DbSet<SectorsView> SectorsViews { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierPaymentDate> SupplierPaymentDates { get; set; }

    public virtual DbSet<SupplierTotalAmount> SupplierTotalAmounts { get; set; }

    public virtual DbSet<SupplierTransaction> SupplierTransactions { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTotalAmount> UserTotalAmounts { get; set; }

    public virtual DbSet<UserTransaction> UserTransactions { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=.;Database=Daftari;User Id=sa;Password=sa123456;Trusted_Connection=True;TrustServerCertificate=True;");

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
			entity.ToTable(tb => tb.HasTrigger("trg_AfterDelete_Client")); 

			entity.HasKey(e => e.ClientId).HasName("PK__Clients__E67E1A248096CCF6");

            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Person).WithOne(p => p.Client)
                .HasForeignKey<Client>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Clients__PersonI__4E88ABD4");

            entity.HasOne(d => d.User).WithMany(p => p.Clients)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Clients__UserId__4F7CD00D");

		});

        modelBuilder.Entity<ClientPaymentDate>(entity =>
        {
            entity.HasKey(e => e.ClientPaymentDateId).HasName("PK__ClientPa__2267EA197BED809A");

            entity.HasOne(d => d.Client).WithOne(p => p.ClientPaymentDate)
                .HasForeignKey<Client>(d => d.ClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ClientPay__Clien__787EE5A0");


			entity.HasOne(d => d.PaymentDate).WithOne(p => p.ClientPaymentDate)
                .HasForeignKey<PaymentDate>(d => d.PaymentDateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClientPay__Payme__797309D9");

            entity.HasOne(d => d.User).WithOne(p => p.ClientPaymentDate)
                .HasForeignKey<User>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ClientPay__UserI__778AC167");
        });

        modelBuilder.Entity<ClientTotalAmount>(entity =>
        {
            entity.HasKey(e => e.ClientTotalAmountId).HasName("PK__ClientTo__8CD4709080842B5F");

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Client).WithOne(p => p.ClientTotalAmount)
                .HasForeignKey<Client>(d => d.ClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ClientTot__Clien__7C4F7684");

            entity.HasOne(d => d.User).WithOne(p => p.ClientTotalAmount)
                .HasForeignKey<User>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ClientTot__UserI__7D439ABD");
        });

        modelBuilder.Entity<ClientTransaction>(entity =>
        {
            entity.HasKey(e => e.ClientTransactionId).HasName("PK__ClientTr__93829C4302632657");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientTransactions)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ClientTra__Clien__6383C8BA");

            entity.HasOne(d => d.Transaction).WithOne(p => p.ClientTransaction)
                .HasForeignKey< Transaction>(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClientTra__Trans__619B8048");

            entity.HasOne(d => d.User).WithMany(p => p.ClientTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ClientTra__UserI__628FA481");
        });

		modelBuilder.Entity<PaymentDate>(entity =>
		{
			entity.Property(e => e.PaymentDateId)
             .ValueGeneratedOnAdd();

			// Use date type for DateOfPayment and apply default value logic
			entity.Property(e => e.DateOfPayment)
				.HasDefaultValueSql("DATEADD(DAY, 30, GETDATE())") // Fix the default SQL expression
				.HasColumnType("datetime");  // Use "date" instead of "datetime" to match your table definition

			// Notes field with maximum length
			entity.Property(e => e.Notes)
				.HasMaxLength(500);

			// Default value for PaymentMethodId
			entity.Property(e => e.PaymentMethodId)
				.HasDefaultValue((byte)1);

			// Foreign Key relationship with PaymentMethod table
			entity.HasOne(d => d.PaymentMethod)
				.WithMany(p => p.PaymentDates)
				.HasForeignKey(d => d.PaymentMethodId)
				.OnDelete(DeleteBehavior.ClientSetNull)  // Set null when deleted (or adjust this based on your requirement)
				.HasConstraintName("FK__PaymentDa__Payme__6FE99F9F");
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

        modelBuilder.Entity<SectorsView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SectorsView");

            entity.Property(e => e.SectorName).HasMaxLength(50);
            entity.Property(e => e.SectorTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
			entity.ToTable(tb => tb.HasTrigger("trg_AfterDelete_Supplier"));
			entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B49241C1FB");

            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Person).WithOne(p => p.Supplier)
                .HasForeignKey<Supplier>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Suppliers__Perso__52593CB8");

            entity.HasOne(d => d.User).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Suppliers__UserI__534D60F1");
        });

        modelBuilder.Entity<SupplierPaymentDate>(entity =>
        {
            entity.HasKey(e => e.SupplierPaymentDateId).HasName("PK__Supplier__59A467047D5C4161");

            entity.HasOne(d => d.PaymentDate).WithOne(p => p.SupplierPaymentDate)
                .HasForeignKey<PaymentDate>(d => d.PaymentDateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierP__Payme__74AE54BC");

            entity.HasOne(d => d.Supplier).WithOne(p => p.SupplierPaymentDate)
                .HasForeignKey<Supplier>(d => d.SupplierId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SupplierP__Suppl__73BA3083");

            entity.HasOne(d => d.User).WithMany(p => p.SupplierPaymentDates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SupplierP__UserI__72C60C4A");
        });

        modelBuilder.Entity<SupplierTotalAmount>(entity =>
        {
            entity.HasKey(e => e.SupplierTotalAmountId).HasName("PK__Supplier__266E965A1940BD06");

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Supplier).WithOne(p => p.SupplierTotalAmount)
                .HasForeignKey<Supplier>(d => d.SupplierId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SupplierT__Suppl__02084FDA");

            entity.HasOne(d => d.User).WithMany(p => p.SupplierTotalAmounts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SupplierT__UserI__02FC7413");
        });

        modelBuilder.Entity<SupplierTransaction>(entity =>
        {
            entity.HasKey(e => e.SupplierTransactionId).HasName("PK__Supplier__E905F179691AD6C4");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierTransactions)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SupplierT__Suppl__693CA210");

            entity.HasOne(d => d.Transaction).WithOne(p => p.SupplierTransaction)
                .HasForeignKey<Transaction>(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierT__Trans__6754599E");

            entity.HasOne(d => d.User).WithMany(p => p.SupplierTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SupplierT__UserI__68487DD7");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6BB9383060");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ImageType).HasMaxLength(10);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.TransactionTypeId)
			    .OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK__Transacti__Trans__5812160E");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.TransactionTypeId).HasName("PK__Transact__20266D0B72E9F8D5");

            entity.Property(e => e.TransactionTypeId).ValueGeneratedOnAdd();
            entity.Property(e => e.TransactionTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
			entity.ToTable(tb => tb.HasTrigger("trg_AfterDelete_User"));

			entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CEE2123DA");

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

            entity.HasOne(d => d.Person).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__PersonId__46E78A0C");

            entity.HasOne(d => d.Sector).WithMany(p => p.Users)
                .HasForeignKey(d => d.SectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__SectorId__47DBAE45");
        });

        modelBuilder.Entity<UserTotalAmount>(entity =>
        {
            entity.HasKey(e => e.UserTotalAmountId).HasName("PK__UserTota__72480AB68FD9160C");

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.UserTotalAmounts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__UserTotal__UserI__07C12930");
        });

        modelBuilder.Entity<UserTransaction>(entity =>
        {
            entity.HasKey(e => e.UserTransactionId).HasName("PK__UserTran__55CF47FF2776C827");

            entity.HasOne(d => d.Transaction).WithOne(p => p.UserTransaction)
                .HasForeignKey<Transaction>(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserTrans__Trans__5CD6CB2B");

            entity.HasOne(d => d.User).WithMany(p => p.UserTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__UserTrans__UserI__5DCAEF64");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
