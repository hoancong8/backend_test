using System;
using System.Collections.Generic;

namespace test.Models;

public partial class Postimage
{
    public Guid Id { get; set; }

    public string ImageUrl { get; set; } = null!;

    public Guid PostId { get; set; }

    public virtual Post Post { get; set; } = null!;
}
