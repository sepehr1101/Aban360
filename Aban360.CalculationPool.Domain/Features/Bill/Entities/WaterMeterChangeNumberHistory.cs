using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(WaterMeterChangeNumberHistory))]
public class WaterMeterChangeNumberHistory
{
    public long Id { get; set; }

    public int Consumption { get; set; }

    public float ConstumptionAverage { get; set; }

    public short ChangeMeterReasonId { get; set; }

    public long InvoiceId { get; set; }

    public virtual Invoice Invoice{ get; set; } = null!;
}
