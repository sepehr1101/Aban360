using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(InvoinceLineItemInsertMode))]
public class InvoinceLineItemInsertMode
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<InvoiceLineItem> InvoiceLineItems { get; set; } = new List<InvoiceLineItem>();
}
