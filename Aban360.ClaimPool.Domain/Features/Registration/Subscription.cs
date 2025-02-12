using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Registration;

[Table(nameof(Subscription))]
public class Subscription
{
    public int Id { get; set; }

    public string? ReadingNumber { get; set; }

    public int CustomerNumber { get; set; }

    public string BillId { get; set; } = null!;

    public int EstateId { get; set; }

    public int WaterMeterId { get; set; }

    public short UseStateId { get; set; }

    public Guid UserId { get; set; }

    public int? PreviousId { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string InsertLogInfo { get; set; } = null!;

    public string? RemoveLogInfo { get; set; }

    public string Hash { get; set; } = null!;

    public virtual Estate Estate { get; set; } = null!;

    public virtual Estate? Previous { get; set; }

    public virtual UseState UseState { get; set; } = null!;

    public virtual WaterMeter WaterMeter { get; set; } = null!;
}
