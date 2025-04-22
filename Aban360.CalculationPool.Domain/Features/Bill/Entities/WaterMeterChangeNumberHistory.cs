using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(WaterMeterChangeNumberHistory))]
public class WaterMeterChangeNumberHistory
{
    public long Id { get; set; }

    public long Consumption { get; set; }

    public long ConstumptionAverage { get; set; }

    public short ChangeMeterReasonId { get; set; }

    public long InvoiceInstallmentId { get; set; }

    public virtual InvoiceInstallment InvoiceInstallment { get; set; } = null!;
}
