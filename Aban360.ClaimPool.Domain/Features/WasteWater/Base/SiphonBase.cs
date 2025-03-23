using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Domain.Features.WasteWater.Base;

public class SiphonBase
{
    public int Id { get; set; }

    public string? InstallationLocation { get; set; }

    public string? InstallationDate { get; set; }

    public short SiphonDiameterId { get; set; }

    public short SiphonTypeId { get; set; }

    public short SiphonMaterialId { get; set; }

    public Guid UserId { get; set; }

    public int? PreviousId { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string InsertLogInfo { get; set; } = null!;

    public string? RemoveLogInfo { get; set; }

    public string Hash { get; set; } = null!;
}
