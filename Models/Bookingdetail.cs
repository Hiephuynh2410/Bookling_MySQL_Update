using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Bookingdetail
{
    public int BookingId { get; set; }

    public int ServiceId { get; set; }

    public decimal? Price { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
