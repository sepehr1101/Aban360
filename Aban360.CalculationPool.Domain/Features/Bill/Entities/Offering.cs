using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(Offering))]
public class Offering
{
    public short Id { get; set; }

    public short OfferingUnitId { get; set; }

    public short OfferingGroupId { get; set; }

    public string Title { get; set; } = null!;

    public bool InstallmentOption { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<InvoiceLineItem> InvoiceLineItems { get; set; } = new List<InvoiceLineItem>();

    public virtual OfferingGroup OfferingGroup { get; set; } = null!;

    public virtual OfferingUnit OfferingUnit { get; set; } = null!;
}
