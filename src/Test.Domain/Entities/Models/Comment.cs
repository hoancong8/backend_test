using System;
using System.Collections.Generic;

namespace test.src.Test.Domain.Entities.Models;

public partial class Comment
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public Guid UserId { get; set; }

    public Guid PostId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ImageUrl { get; set; }

    public Guid? ParentCommentId { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
