using System;
using System.Collections.Generic;

namespace test.Models;

public partial class Post
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public string? ImageUrl { get; set; }

    public string? VideoUrl { get; set; }

    public Guid UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Postimage> Postimages { get; set; } = new List<Postimage>();

    public virtual ICollection<Postvideo> Postvideos { get; set; } = new List<Postvideo>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
