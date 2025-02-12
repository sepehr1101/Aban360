using Aban360.ClaimPool.Domain.Features.Land;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.WasteWater;

[Table(nameof(SiphonType))]
public partial class SiphonType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Siphon> Siphons { get; set; } = new List<Siphon>();
}
