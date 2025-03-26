using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.TimeTable.Entites;

[Table(nameof(UsageLevel1))]
public class UsageLevel1
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<UsageLevel2> UsageLevel2s { get; set; } = new List<UsageLevel2>();
}
