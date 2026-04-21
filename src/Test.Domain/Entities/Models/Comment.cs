using System;
using System.Collections.Generic;

namespace test.Models;

public partial class Comment
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public Guid UserId { get; set; }

    public Guid PostId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
