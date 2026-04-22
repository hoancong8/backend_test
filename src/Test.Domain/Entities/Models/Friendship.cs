using System;
using System.Collections.Generic;

namespace test.src.Test.Domain.Entities.Models;

public partial class Friendship
{
    public Guid Id { get; set; }

    public Guid? RequesterId { get; set; }

    public Guid? AddresseeId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual User? Addressee { get; set; }

    public virtual User? Requester { get; set; }
}
