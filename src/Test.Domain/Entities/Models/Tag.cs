using System;
using System.Collections.Generic;

namespace test.src.Test.Domain.Entities.Models;

public partial class Tag
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
