using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(Estate), Schema = TableSchema.Name)]
public class Estate: EstateBase
{

}
