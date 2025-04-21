using Aban360.CalculationPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(InvoiceLineItem))]
public class InvoiceLineItem
{
    public long Id { get; set; }

    public long InvoiceId { get; set; }

    public short OfferingId { get; set; }

    public InvoiceLineItemInsertModeEnum InvoiceLineItemInsertModeId { get; set; }

    public long Amount { get; set; }

    public int Quanity { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual InvoiceLineItemInsertMode InvoinceLineItemInsertMode { get; set; } = null!;

    public virtual Offering Offering { get; set; } = null!;
}
