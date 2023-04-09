﻿using System;
using System.Collections.Generic;

namespace CI_PlatForm.Entities.Models;

public partial class Country
{
    public long CountryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Iso { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? CountryName { get; set; }

    public virtual ICollection<City> Cities { get; } = new List<City>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
