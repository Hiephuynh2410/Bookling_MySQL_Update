using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Branch
{
    public int BranchId { get; set; }

    public string? Address { get; set; }

    public string? Hotline { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
