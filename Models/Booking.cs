using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? ClientId { get; set; }

    public int? StaffId { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public DateTime? DateTime { get; set; }

    public string? Note { get; set; }

    public bool? Status { get; set; }

    public int? ComboId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? BranchId { get; set; }

    public virtual ICollection<Bookingdetail> Bookingdetails { get; set; } = new List<Bookingdetail>();

    public virtual Branch? Branch { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Combo? Combo { get; set; }

    public virtual Staff? Staff { get; set; }
}
