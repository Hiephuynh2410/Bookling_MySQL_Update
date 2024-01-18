using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public TimeOnly? Time { get; set; }

    public virtual ICollection<Scheduledetail> Scheduledetails { get; set; } = new List<Scheduledetail>();
}
