using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.MeterPool.Domain.Features.Management.Entities;

[Table(nameof(ReadingPeriodType))]
public class ReadingPeriodType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short Days { get; set; }

    public short ClientOrder { get; set; }

    public bool IsEnabled { get; set; }

    public short HeadquartersId { get; set; }

    public string HeadquartersTitle { get; set; } = null!;

    public virtual ICollection<ReadingPeriod> ReadingPeriods { get; set; } = new List<ReadingPeriod>();
}
