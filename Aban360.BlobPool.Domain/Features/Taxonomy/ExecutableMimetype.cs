using System;
using System.Collections.Generic;

namespace Aban360.BlobPool.Domain.Features.Taxonomy;

public partial class ExecutableMimetype
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public bool StreamingOption { get; set; }

    public bool FrontendExecutor { get; set; }
}
