using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
