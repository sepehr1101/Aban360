using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features._Base.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(Flat), Schema = TableSchema.Name)]
public class Flat: FlatBase
{   
    public virtual Estate Estate { get; set; } = null!;
}
