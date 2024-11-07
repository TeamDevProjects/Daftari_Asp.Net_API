using System;
using System.Collections.Generic;

namespace Daftari.Entities;

public partial class User
{
    public int UserId { get; set; }

    public int PersonId { get; set; }

    public string StoreName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public byte SectorId { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public byte BusinessTypeId { get; set; }

    public string UserType { get; set; } = null!;

    public virtual BusinessType BusinessType { get; set; } = null!;

    public virtual ICollection<ClientPaymentDate> ClientPaymentDates { get; set; } = new List<ClientPaymentDate>();

    public virtual ICollection<ClientTransaction> ClientTransactions { get; set; } = new List<ClientTransaction>();

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual Person Person { get; set; } = null!;

    public virtual Sector Sector { get; set; } = null!;

    public virtual ICollection<SupplierPaymentDate> SupplierPaymentDates { get; set; } = new List<SupplierPaymentDate>();

    public virtual ICollection<SupplierTransaction> SupplierTransactions { get; set; } = new List<SupplierTransaction>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    public virtual ICollection<UserTransaction> UserTransactions { get; set; } = new List<UserTransaction>();
}
