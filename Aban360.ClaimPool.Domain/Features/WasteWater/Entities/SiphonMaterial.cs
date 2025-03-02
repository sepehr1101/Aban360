using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

[Table(nameof(SiphonMaterial), Schema = TableSchema.Name)]
public class SiphonMaterial
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Siphon> Siphons { get; set; } = new List<Siphon>();
}
