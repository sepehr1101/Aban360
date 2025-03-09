using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Metering.Entities;

[Table(nameof(SubscriptionType),Schema =TableSchema.Name)]
public class SubscriptionType
{
    public SubscriptionTypeEnum Id { get; set; }

    public string Title { get; set; } = null!;
}
