using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Cart
{
    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Client User { get; set; } = null!;
}
