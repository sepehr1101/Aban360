using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.MeterPool.Domain.Features.Management.Entities;

[Table(nameof(ReadingPeriod))]
public class ReadingPeriod
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ReadingPeriodTypeId { get; set; }

    public short ClientOrder { get; set; }

    public virtual ReadingPeriodType ReadingPeriodType { get; set; } = null!;
}
