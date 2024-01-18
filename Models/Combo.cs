using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Combo
{
    public int ComboId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Combodetail> Combodetails { get; set; } = new List<Combodetail>();
}
