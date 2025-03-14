using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Offering
{
    public short Id { get; set; }

    public short OfferingUnitId { get; set; }

    public short OfferingGroupId { get; set; }

    public string Title { get; set; } = null!;

    public bool InstallmentOption { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<CompanyServiceOffering> CompanyServiceOfferings { get; set; } = new List<CompanyServiceOffering>();

    public virtual ICollection<InvoiceLineItem> InvoiceLineItems { get; set; } = new List<InvoiceLineItem>();

    public virtual OfferingGroup OfferingGroup { get; set; } = null!;

    public virtual OfferingUnit OfferingUnit { get; set; } = null!;

    public virtual ICollection<Tariff> Tariffs { get; set; } = new List<Tariff>();
}
