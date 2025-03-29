using Aban360.ClaimPool.Domain.Features._Base;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    public class RequestSiphon: SiphonBase
    {

        public virtual RequestWaterMeter RequestWaterMeter { get; set; }

        public virtual ICollection<RequestSiphon> InversePrevious { get; set; } = new List<RequestSiphon>();

        public virtual RequestSiphon? Previous { get; set; }

        public virtual SiphonDiameter SiphonDiameter { get; set; } = null!;

        public virtual SiphonMaterial SiphonMaterial { get; set; } = null!;

        public virtual SiphonType SiphonType { get; set; } = null!;
        public virtual ICollection<RequestWaterMeterSiphon> RequestWaterMeterSiphons { get; set; } = new List<RequestWaterMeterSiphon>();
    }
}
