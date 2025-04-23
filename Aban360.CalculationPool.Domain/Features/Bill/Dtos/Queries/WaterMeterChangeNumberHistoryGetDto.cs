using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record WaterMeterChangeNumberHistoryGetDto
    {
        public long Id { get; set; }

        public int Consumption { get; set; }

        public float ConstumptionAverage { get; set; }

        public short ChangeMeterReasonId { get; set; }

        public long InvoicetId { get; set; }

        public virtual Invoice Invoice{ get; set; } = null!;
    }
}
