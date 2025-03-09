using Aban360.CalculationPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(InvoiceLineItemInsertMode))]
public class InvoiceLineItemInsertMode
{
    public InvoiceLineItemInsertModeEnum Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<InvoiceLineItem> InvoiceLineItems { get; set; } = new List<InvoiceLineItem>();
}
