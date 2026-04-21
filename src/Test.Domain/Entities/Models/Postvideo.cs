using System;
using System.Collections.Generic;

namespace test.Models;

public partial class Postvideo
{
    public Guid Id { get; set; }

    public string VideoUrl { get; set; } = null!;

    public Guid PostId { get; set; }

    public virtual Post Post { get; set; } = null!;
}
