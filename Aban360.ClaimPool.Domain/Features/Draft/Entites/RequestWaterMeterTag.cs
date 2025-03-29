using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    public class RequestWaterMeterTag  // ToDo: Has Base?
    {
        public int Id { get; set; }

        public int WaterMeterId { get; set; }

        public short WaterMeterTagDefinitionId { get; set; }

        public string? Value { get; set; }

        public virtual RequestWaterMeter RequestWaterMeter{ get; set; } = null!;

        public virtual WaterMeterTagDefinition WaterMeterTagDefinition { get; set; } = null!;
    }
}
