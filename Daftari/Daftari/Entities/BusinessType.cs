﻿using System.Text.Json.Serialization;

namespace Daftari.Entities;

public partial class BusinessType
{
    public byte BusinessTypeId { get; set; }

    public string BusinessTypeName { get; set; } = null!;

	[JsonIgnore]
	public virtual ICollection<User> Users { get; set; } = new List<User>();
}
