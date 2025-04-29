using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class RequestSiphon
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

    public virtual ICollection<RequestSiphon> InversePrevious { get; set; } = new List<RequestSiphon>();

    public virtual RequestSiphon? Previous { get; set; }

    public virtual ICollection<RequestWaterMeterSiphon> RequestWaterMeterSiphons { get; set; } = new List<RequestWaterMeterSiphon>();

    public virtual SiphonDiameter SiphonDiameter { get; set; } = null!;

    public virtual SiphonMaterial SiphonMaterial { get; set; } = null!;

    public virtual SiphonType SiphonType { get; set; } = null!;
}
