using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(ConstructionType), Schema = TableSchema.Name)]
public class ConstructionType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Estate> Estates { get; set; } = new List<Estate>();
}
