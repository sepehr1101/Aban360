using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.People.Entities;

[Table(nameof(Individual), Schema = TableSchema.Name)]
public class Individual: IndividualBase
{
   
}
