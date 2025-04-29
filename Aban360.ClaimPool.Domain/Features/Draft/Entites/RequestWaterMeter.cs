using Aban360.ClaimPool.Domain.Features._Base.Entities;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    [Table(nameof(RequestWaterMeter))]
    public class RequestWaterMeter : WaterMeterBase
    {
        [ForeignKey(nameof(EstateId))]
        public virtual RequestEstate RequestEstate { get; set; } = null!;
        //public virtual ICollection<RequestWaterMeter> InverseParent { get; set; } = new List<RequestWaterMeter>();

        public virtual MeterDiameter MeterDiameter { get; set; } = null!;

        public virtual MeterMaterial MeterMaterial { get; set; } = null!;

        public virtual MeterProducer MeterProducer { get; set; } = null!;

        public virtual MeterType MeterType { get; set; } = null!;

        public virtual MeterUseType MeterUseType { get; set; } = null!;

        /* [ForeignKey(nameof(ParentId))]
         public virtual RequestWaterMeter? Parent { get; set; }*/

        public virtual UseState UseState { get; set; } = null!;
        public virtual SubscriptionType SubscriptionType { get; set; } = null!;

        public virtual ICollection<RequestWaterMeterSiphon> WaterMeterSiphons { get; set; } = new List<RequestWaterMeterSiphon>();
        public virtual ICollection<RequestWaterMeterTag> WaterMeterTags { get; set; } = new List<RequestWaterMeterTag>();
        public virtual WaterMeterInstallationStructure WaterMeterInstallationStructure { get; set; }

    }
}
