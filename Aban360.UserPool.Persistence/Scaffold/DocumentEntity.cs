using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class DocumentEntity
{
    public long Id { get; set; }

    public Guid DocumentId { get; set; }

    public long TableId { get; set; }

    public short RelationEntityId { get; set; }
}
