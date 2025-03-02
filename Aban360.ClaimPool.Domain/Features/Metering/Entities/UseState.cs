using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Metering.Entities;

[Table(nameof(UseState), Schema = TableSchema.Name)]
public class UseState
{
    public UseStateEnum Id { get; set; }

    public string Title { get; set; } = null!;
}
