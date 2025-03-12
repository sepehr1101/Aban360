using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class InvoiceInstallment
{
    public long Id { get; set; }

    public long InvoiceId { get; set; }

    public long Amount { get; set; }

    public string DueDateJalali { get; set; } = null!;

    public DateTime DueDateTime { get; set; }

    public int InstallmentOrder { get; set; }

    public string? BillId { get; set; }

    public string? PaymentId { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;
}
