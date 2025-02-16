using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class WaterMeter
{
    public int Id { get; set; }

    public string? ReadingNumber { get; set; }

    public int CustomerNumber { get; set; }

    public string BillId { get; set; } = null!;

    public int EstateId { get; set; }

    public short UseStateId { get; set; }

    public string? InstallationLocation { get; set; }

    public string? BodySerial { get; set; }

    public string? InstallationDate { get; set; }

    public string? ProductDate { get; set; }

    public string? GuaranteeDate { get; set; }

    public short MeterDiameterId { get; set; }

    public short MeterProducerId { get; set; }

    public short MeterTypeId { get; set; }

    public short MeterMaterialId { get; set; }

    public short MeterUseTypeId { get; set; }

    public int? ParentId { get; set; }

    public Guid UserId { get; set; }

    public int? PreviousId { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string InsertLogInfo { get; set; } = null!;

    public string? RemoveLogInfo { get; set; }

    public string Hash { get; set; } = null!;

    public virtual Estate Estate { get; set; } = null!;

    public virtual ICollection<WaterMeter> InverseParent { get; set; } = new List<WaterMeter>();

    public virtual ICollection<WaterMeter> InversePrevious { get; set; } = new List<WaterMeter>();

    public virtual MeterDiameter MeterDiameter { get; set; } = null!;

    public virtual MeterMaterial MeterMaterial { get; set; } = null!;

    public virtual MeterProducer MeterProducer { get; set; } = null!;

    public virtual MeterType MeterType { get; set; } = null!;

    public virtual MeterUseType MeterUseType { get; set; } = null!;

    public virtual WaterMeter? Parent { get; set; }

    public virtual WaterMeter? Previous { get; set; }

    public virtual UseState UseState { get; set; } = null!;

    public virtual ICollection<WaterMeterSiphon> WaterMeterSiphons { get; set; } = new List<WaterMeterSiphon>();

    public virtual ICollection<WaterMeterTag> WaterMeterTags { get; set; } = new List<WaterMeterTag>();
}
