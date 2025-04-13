using Aban360.ClaimPool.Domain.Features._Base.Entities;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    [Table(nameof(RequestSiphon))]
    public class RequestSiphon: SiphonBase
    {
        public virtual ICollection<RequestSiphon> InversePrevious { get; set; } = new List<RequestSiphon>();

        public virtual RequestSiphon? Previous { get; set; }

        public virtual SiphonDiameter SiphonDiameter { get; set; } = null!;

        public virtual SiphonMaterial SiphonMaterial { get; set; } = null!;

        public virtual SiphonType SiphonType { get; set; } = null!;
        public virtual ICollection<RequestWaterMeterSiphon> WaterMeterSiphons { get; set; } = new List<RequestWaterMeterSiphon>();
    }
}
