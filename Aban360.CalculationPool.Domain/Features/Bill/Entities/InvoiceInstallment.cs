using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(InvoiceInstallment))]
public class InvoiceInstallment
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
