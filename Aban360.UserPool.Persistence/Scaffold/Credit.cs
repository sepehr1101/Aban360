using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Credit
{
    public long Id { get; set; }

    public string BillId { get; set; } = null!;

    public string PaymentId { get; set; } = null!;

    public long InvoiceId { get; set; }

    public long InvoiceInstallmentId { get; set; }

    public long Amount { get; set; }

    public short UploaderId { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string InsertLogInfo { get; set; } = null!;

    public string? RemoveLogInfo { get; set; }

    public string Hash { get; set; } = null!;

    public virtual Uploader Uploader { get; set; } = null!;
}
