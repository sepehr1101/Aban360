using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.WasteWater.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

[Table(nameof(Siphon), Schema = TableSchema.Name)]
public class Siphon: SiphonBase
{
}
