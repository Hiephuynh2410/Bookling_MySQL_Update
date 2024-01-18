using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public bool? Status { get; set; }

    public int? ServiceTypeId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<Bookingdetail> Bookingdetails { get; set; } = new List<Bookingdetail>();

    public virtual ICollection<Combodetail> Combodetails { get; set; } = new List<Combodetail>();

    public virtual Servicetype? ServiceType { get; set; }
}
