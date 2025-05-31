using Aban360.Common.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    [Table(nameof(RequestTracking))]
    public class RequestTracking:IHashableEntity
    {
        public int Id { get; set; }
        public int WaterMeterId { get; set; }
        public short Status { get; set; }
        public Guid UserId { get; set; }

        public virtual RequestWaterMeter WaterMeter { get; set; }
    }
}
