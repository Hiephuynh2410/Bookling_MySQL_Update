using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Provider
{
    public int ProviderId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
