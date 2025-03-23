using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.Metering.Base;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Metering.Entities
{
    [Table(nameof(WaterMeter), Schema = TableSchema.Name)]
    public class WaterMeter: WaterMeterBase
    {      
        public virtual Estate Estate { get; set; } = null!;
        public virtual ICollection<WaterMeter> InverseParent { get; set; } = new List<WaterMeter>();

        //public virtual ICollection<WaterMeter> InversePrevious { get; set; } = new List<WaterMeter>();

        public virtual MeterDiameter MeterDiameter { get; set; } = null!;

        public virtual MeterMaterial MeterMaterial { get; set; } = null!;

        public virtual MeterProducer MeterProducer { get; set; } = null!;

        public virtual MeterType MeterType { get; set; } = null!;

        public virtual MeterUseType MeterUseType { get; set; } = null!;

        [ForeignKey(nameof(ParentId))]
        public virtual WaterMeter? Parent { get; set; }

        //[ForeignKey(nameof(PreviousId))]
        //public virtual WaterMeter? Previous { get; set; }

        public virtual UseState UseState { get; set; } = null!;
        public virtual SubscriptionType SubscriptionType { get; set; } = null!;

        public virtual ICollection<WaterMeterSiphon> WaterMeterSiphons { get; set; } = new List<WaterMeterSiphon>();
        public virtual ICollection<WaterMeterTag> WaterMeterTags { get; set; } = new List<WaterMeterTag>();

    }
}
