using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Bill
{
    public int BillId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime Date { get; set; }

    public int? ClientId { get; set; }

    public virtual ICollection<Billdetail> Billdetails { get; set; } = new List<Billdetail>();

    public virtual Client? Client { get; set; }
}
