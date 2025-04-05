using Aban360.ClaimPool.Domain.Features._Base.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites;

[Table(nameof(RequestIndividualTag))]
public class RequestIndividualTag: IndividualTagBase
{
    [ForeignKey(nameof(IndividualId))]
    public virtual RequestIndividual RequestIndividual { get; set; } = null!;

}
