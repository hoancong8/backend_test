using System;
using System.Collections.Generic;

namespace test.src.Test.Domain.Entities.Models;

public partial class Efmigrationshistory
{
    public string MigrationId { get; set; } = null!;

    public string ProductVersion { get; set; } = null!;
}
