using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Daftari.Entities.Views;

[Keyless]
public partial class UsersView
{
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? Address { get; set; }

    public string StoreName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? SectorName { get; set; }

    public string? SectorTypeName { get; set; }

    public string? BusinessTypeName { get; set; }

    public string UserType { get; set; } = null!;

    public decimal? TotalAmount { get; set; }
}
