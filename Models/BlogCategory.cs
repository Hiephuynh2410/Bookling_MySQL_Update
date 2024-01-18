using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class BlogCategory
{
    public int BlogCategoryId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
}
