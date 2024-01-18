﻿using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Combodetail
{
    public int ComboId { get; set; }

    public int ServiceId { get; set; }

    public decimal? Price { get; set; }

    public virtual Combo Combo { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
