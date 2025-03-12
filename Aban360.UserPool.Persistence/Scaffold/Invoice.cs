using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Invoice
{
    public long Id { get; set; }

    public short InvoiceTypeId { get; set; }

    public short InvoiceStatusId { get; set; }

    public long Amount { get; set; }

    public short OfferingCount { get; set; }

    public short DepositRate { get; set; }

    public short InstallmentCount { get; set; }

    public virtual ICollection<InvoiceInstallment> InvoiceInstallments { get; set; } = new List<InvoiceInstallment>();

    public virtual ICollection<InvoiceLineItem> InvoiceLineItems { get; set; } = new List<InvoiceLineItem>();

    public virtual InvoiceStatus InvoiceStatus { get; set; } = null!;

    public virtual InvoiceType InvoiceType { get; set; } = null!;
}
