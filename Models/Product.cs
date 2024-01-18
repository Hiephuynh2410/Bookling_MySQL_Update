using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public int? ProductTypeId { get; set; }

    public string? Image { get; set; }

    public int? ProviderId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public int? Sold { get; set; }

    public virtual ICollection<Billdetail> Billdetails { get; set; } = new List<Billdetail>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Producttype? ProductType { get; set; }

    public virtual Provider? Provider { get; set; }
}
