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

    public int SectorId { get; set; }

    public virtual ICollection<ClientTransaction> ClientTransactions { get; set; } = new List<ClientTransaction>();

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual Person Person { get; set; } = null!;

    public virtual Sector Sector { get; set; } = null!;

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    public virtual ICollection<UserTransaction> UserTransactions { get; set; } = new List<UserTransaction>();
}
