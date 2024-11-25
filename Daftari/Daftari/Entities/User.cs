using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

	[JsonIgnore]
	public virtual BusinessType BusinessType { get; set; } = null!;

	[JsonIgnore]
	public virtual ICollection<ClientPaymentDate> ClientPaymentDates { get; set; } = new List<ClientPaymentDate>();

	[JsonIgnore]
	public virtual ICollection<ClientTotalAmount> ClientTotalAmounts { get; set; } = new List<ClientTotalAmount>();

	[JsonIgnore]
	public virtual ICollection<ClientTransaction> ClientTransactions { get; set; } = new List<ClientTransaction>();

	[JsonIgnore]
	public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

	[JsonIgnore]
	public virtual Person Person { get; set; } = null!;

	[JsonIgnore]
	public virtual Sector Sector { get; set; } = null!;

	[JsonIgnore]
	public virtual ICollection<SupplierPaymentDate> SupplierPaymentDates { get; set; } = new List<SupplierPaymentDate>();

	[JsonIgnore]
	public virtual ICollection<SupplierTotalAmount> SupplierTotalAmounts { get; set; } = new List<SupplierTotalAmount>();

	[JsonIgnore]
	public virtual ICollection<SupplierTransaction> SupplierTransactions { get; set; } = new List<SupplierTransaction>();

	[JsonIgnore]
	public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

	[JsonIgnore]
	public virtual ICollection<UserTotalAmount> UserTotalAmounts { get; set; } = new List<UserTotalAmount>();
	
	[JsonIgnore]
	public virtual ICollection<UserTransaction> UserTransactions { get; set; } = new List<UserTransaction>();
}
