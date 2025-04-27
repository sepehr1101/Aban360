using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class InvoiceLineItem
{
    public long Id { get; set; }

    public long InvoiceId { get; set; }

    public short OfferingId { get; set; }

    public short InvoiceLineItemInsertModeId { get; set; }

    public long Amount { get; set; }

    public int Quanity { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual InvoiceLineItemInsertMode InvoiceLineItemInsertMode { get; set; } = null!;

    public virtual Offering Offering { get; set; } = null!;
}
