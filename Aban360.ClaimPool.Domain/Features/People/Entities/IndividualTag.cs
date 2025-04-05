using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features._Base.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.People.Entities;

[Table(nameof(IndividualTag), Schema = TableSchema.Name)]
public class IndividualTag: IndividualTagBase
{
    [ForeignKey(nameof(IndividualId))]
    public virtual Individual Individual { get; set; } = null!;

}
